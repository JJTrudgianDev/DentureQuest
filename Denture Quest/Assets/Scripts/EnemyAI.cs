using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class EnemyAI : MonoBehaviour
{

    public Transform[] patrolPoints;
    private int currentPoint = 0;
    private NavMeshAgent agent;
    public GameObject player;
    public bool playerInRange = false;

    public enum ActivityType { None, BrushTeeth, LookOutWindow, DoWork, UsePhone, Dishes }
    public ActivityType[] activities;
    public float activityTime = 10f;
    private float timer = 0f;
    private bool performingActivity = false;


    // LINE OF SIGHT
    public bool hasSight = false;
    public bool drawDebugInfo = false;
    public Vector3 headPosition = new Vector3(0f, 1.8f, 0f);
    public Vector3 playerHeadPosition = new Vector3(0f, 0.4f, 0f);
    public float fieldOfViewAngle = 100f;
    public float playerInRangeDistance = 3f;
    private RaycastHit hit;


    public float detectionTime = 5f;
    public float detectionTimer = 0f;
    private bool detectingPlayer = false;
    public GameObject detectionBar;
    public Image alertnessImage;
    public float movementSpeed = 3.5f;
    public float followSpeed = 1f;

    private float timeSinceLastToggle = 0f;
    private float toggleInterval = 0.3f;
    [SerializeField] private Image DamageIndecator;

    public Animator animator;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;
        SelectRandomPoint();
        player = GameObject.FindGameObjectWithTag("Player");




    }

    void Update()
    {
        LineOfSightCheck();

        UpdatePlayerInRange();

        Debug.Log("detectingPlayer = " + detectingPlayer);
        Debug.Log("detectingTimer = " + detectionTimer);

        UpdateActivity();

        UpdateDetection();

        if (animator != null)
        {
            animator.SetFloat("Speed", movementSpeed);
        }

        if (!playerInRange) // Only face the destination if the player is not in range
        {
            FaceDestination();
        }
    }


    public void UpdatePlayerInRange()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < playerInRangeDistance)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
            detectingPlayer = false;
        }
    }

    public void LineOfSightCheck()
    {
        //Debug.Log("Running Line of Sight check.");



        // draw a Linecast from the enemy to the player (or it might hit another collider first while trying to reach the player)
        if (Physics.Linecast(transform.position + headPosition, player.transform.position + playerHeadPosition, out hit))
        {

            //Debug.Log("Did a LineCast and hit: " + hit.collider.name);

            // if the name of the Collider on the player (target) == the name of the Collider the raycast hit first...
            //if (player.GetComponentInChildren<Collider>().name == hit.collider.name)
            if (hit.collider.tag == "Player")
            {

                //Debug.Log("Linecast hit the player.");

                // if it hit the player collider, was the angle of the Linecast within the enemy's fieldOfViewAngle?
                if (Vector3.Angle(player.transform.position - transform.position, transform.forward) <= fieldOfViewAngle)
                {
                    // if the enemy saw the player directly, within their fieldOfViewAngle, draw a green line, and other stuff...
                    if (drawDebugInfo) { Debug.DrawLine(player.transform.position + playerHeadPosition, transform.position + headPosition, Color.green); }
                    hasSight = true;
                    //lastSightingLocation = player.transform.position;
                }
                else
                { // else, would be able to see player unobstructed, but they are out of their fieldOfView
                    hasSight = false;
                    if (drawDebugInfo) { Debug.DrawLine(player.transform.position + playerHeadPosition, transform.position + headPosition, Color.blue); }
                }
            }
            else
            { // else, line of sight was blocked by an object, draw a red line
                if (drawDebugInfo) { Debug.DrawLine(player.transform.position + playerHeadPosition, transform.position + headPosition, Color.red); }
                hasSight = false;
                //Debug.Log("LineCast hit something else instead of player: " + hit.collider.name);
            }
        }

        //Debug.Log("Enemy hasSight = " + hasSight);

        // debug log which collider is hit by sight ray
        //Debug.Log ("Blocked by " + hit.collider.name);    // Output the name of the collider that was hit

    }


    void UpdateActivity()
    {
        if (!performingActivity && agent.remainingDistance <= agent.stoppingDistance && !playerInRange)
            StartActivity();
        if (performingActivity && (timer += Time.deltaTime) >= activityTime)
            EndActivity();
    }


    void UpdateDetection()
    {
        if (detectingPlayer && hasSight)
        {
            detectionTimer = Mathf.Clamp(playerInRange ? detectionTimer + Time.deltaTime : detectionTimer - Time.deltaTime, 0f, detectionTime);
            alertnessImage.fillAmount = detectionTimer / detectionTime;
            if (detectionTimer >= detectionTime) DetectPlayer();
        }
    }


    void OnTriggerEnter(Collider other)
    {
        // if detect something that isn't player, return
        //if (!other.CompareTag("Player")) return;
        //playerInRange = detectingPlayer = true;
    }


    void OnTriggerStay(Collider other)
    {
        // if not yet detecting player
        if (other.gameObject.tag == "Player")
        {
            if (!detectingPlayer)
            {
                // if inside sphere, and hasSight
                if (hasSight)
                {
                    detectingPlayer = true;
                    detectionTimer = 0f;
                    detectionBar.SetActive(true);
                    agent.isStopped = true;
                }
            }
        }


        //if (!other.CompareTag("Player")) return;

        // when closer to player, face them and follow them
        if (detectingPlayer)
        {
            FacePlayer();
            FollowPlayer();
        }


    }


    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        //detectingPlayer = playerInRange = false;
        agent.isStopped = false;
        StartCoroutine(DecreaseDetection());
        EndActivity(); // Immediately resume patrol without waiting
        DamageIndecator.enabled = false;

    }


    IEnumerator DecreaseDetection()
    {
        while (detectionTimer > 0)
        {
            detectionTimer -= Time.deltaTime;
            alertnessImage.fillAmount = detectionTimer / detectionTime;
            yield return null;
        }
        detectionBar.SetActive(false);
    }

    void SelectRandomPoint()
    {
        currentPoint = Random.Range(0, patrolPoints.Length);
        agent.SetDestination(patrolPoints[currentPoint].position);
    }

    void StartActivity()
    {
        performingActivity = true;
        ActivityType activity = activities[currentPoint];
        movementSpeed = 0f;

        switch (activity)
        {
            case ActivityType.None:
                break;
            case ActivityType.BrushTeeth:
                BrushTeeth();
                break;
            case ActivityType.LookOutWindow:
                LookOutWindow();
                break;
            case ActivityType.DoWork:
                DoWork();
                break;
            case ActivityType.UsePhone:
                UsePhone();
                break;
            case ActivityType.Dishes:
                Dishes();
                break;
            default:
                break;
        }
    }

    void FaceDestination()
    {
        Vector3 directionToDestination;

        if (performingActivity) // Face the patrol point's x direction when performing activity
        {
            directionToDestination = patrolPoints[currentPoint].position - transform.position;
        }
        else if (agent.remainingDistance > agent.stoppingDistance) // Face the destination while moving towards it
        {
            directionToDestination = agent.destination - transform.position;
        }
        else // No need to face any direction if not performing activity and agent is at the stopping distance
        {
            return;
        }

        directionToDestination.y = 0;

        if (directionToDestination.magnitude > 0.001f) // Check if the direction vector has a magnitude greater than a small threshold
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToDestination);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }


    void EndActivity()
    {
        performingActivity = false;
        timer = 0f;
        movementSpeed = 3.5f;

        SelectRandomPoint();
    }

    void BrushTeeth()
    {
        movementSpeed = 0;
        Debug.Log("Brushing teeth at patrol point " + currentPoint);
    }
    void LookOutWindow()
    {
        movementSpeed = 0;
        Debug.Log("Looking out window at patrol point " + currentPoint);
    }

    void DoWork()
    {
        movementSpeed = 0;
        Debug.Log("DoWork at patrol point " + currentPoint);
    }

    void UsePhone()
    {
        movementSpeed = 0;
        Debug.Log("UsePhone at patrol point " + currentPoint);
    }

    void Dishes()
    {
        movementSpeed = 0;
        Debug.Log("Dishes at patrol point " + currentPoint);
    }

    void DetectPlayer()
    {
        Debug.Log("Player detected!");
        detectingPlayer = false;
        detectionTimer = 0f;
        detectionBar.SetActive(false);
        SceneManager.LoadScene("LevelFailed");
    }

    void FacePlayer()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        directionToPlayer.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        // Toggle the image on and off every 0.5 seconds
        timeSinceLastToggle += Time.deltaTime;
        if (timeSinceLastToggle >= toggleInterval)
        {
            DamageIndecator.enabled = !DamageIndecator.enabled;
            timeSinceLastToggle = 0f;
        }
    }

    void FollowPlayer()
    {
        agent.isStopped = false;
        agent.speed = followSpeed;
        agent.SetDestination(player.transform.position);
    }

}
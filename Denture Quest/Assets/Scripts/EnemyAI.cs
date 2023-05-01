using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EnemyAI : MonoBehaviour
{

    public Transform[] patrolPoints;
    private int currentPoint = 0;
    private NavMeshAgent agent;
    private GameObject player;
    private bool playerInRange = false;

    public enum ActivityType { None, BrushTeeth, LookOutWindow, DoWork, UsePhone, Dishes }
    public ActivityType[] activities;
    public float activityTime = 10f;
    private float timer = 0f;
    private bool performingActivity = false;

    public float detectionTime = 5f;
    private float detectionTimer = 0f;
    private bool detectingPlayer = false;
    public GameObject detectionBar;
    public Image alertnessImage;
    public float movementSpeed = 3.5f;
    public float followSpeed = 1f;

    private float timeSinceLastToggle = 0f;
    private float toggleInterval = 0.3f;
    [SerializeField] private Image DamageIndecator;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;
        SelectRandomPoint();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        UpdateActivity();
        UpdateDetection();
        if (!playerInRange) // Only face the destination if the player is not in range
        {
            FaceDestination();
        }
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
        if (!detectingPlayer) return;
        detectionTimer = Mathf.Clamp(playerInRange ? detectionTimer + Time.deltaTime : detectionTimer - Time.deltaTime, 0f, detectionTime);
        alertnessImage.fillAmount = detectionTimer / detectionTime;
        if (detectionTimer >= detectionTime) DetectPlayer();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = detectingPlayer = true;
        detectionTimer = 0f;
        detectionBar.SetActive(true);
        agent.isStopped = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        FacePlayer();
        FollowPlayer();
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        detectingPlayer = playerInRange = false;
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
        SelectRandomPoint();
    }

    void BrushTeeth()
    {
        Debug.Log("Brushing teeth at patrol point " + currentPoint);
    }
    void LookOutWindow()
    {
        Debug.Log("Looking out window at patrol point " + currentPoint);
    }

    void DoWork()
    {
        Debug.Log("DoWork at patrol point " + currentPoint);
    }

    void UsePhone()
    {
        Debug.Log("UsePhone at patrol point " + currentPoint);
    }

    void Dishes()
    {
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
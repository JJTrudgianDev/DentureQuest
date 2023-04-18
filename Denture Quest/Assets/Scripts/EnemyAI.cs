using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPoint = 0;
    private NavMeshAgent agent;

    private GameObject player;
    
    // Activities to perform at each patrol point
    public enum ActivityType
    {
        None,
        BrushTeeth,
        LookOutWindow,
        DoWork,
        UsePhone,
        Dishes
    }

    // Assign an activity for each patrol point
    public ActivityType[] activities;

    // Time to stay at each patrol point
    public float activityTime = 10f;
    private float timer = 0f;
    private bool performingActivity = false;

    // Detection mechanics
    public float detectionTime = 5f;
    private float detectionTimer = 0f;
    private bool detectingPlayer = false;
    public GameObject detectionBar;
    public Image alertnessImage;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        SelectRandomPoint();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (!performingActivity && agent.remainingDistance <= agent.stoppingDistance)
        {
            StartActivity();
        }

        if (performingActivity)
        {
            timer += Time.deltaTime;
            if (timer >= activityTime)
            {
                EndActivity();
            }
        }

        if (detectingPlayer)
        {
            detectionTimer += Time.deltaTime;
            alertnessImage.fillAmount = detectionTimer / detectionTime;

            if (detectionTimer >= detectionTime)
            {
                DetectPlayer();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            detectionTimer = 0f;
            detectingPlayer = true;
            detectionBar.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            detectingPlayer = false;
            detectionBar.SetActive(false);
        }
    }

    void SelectRandomPoint()
    {
        // Select a random patrol point
        int randomPoint = Random.Range(0, patrolPoints.Length);
        currentPoint = randomPoint;
        agent.SetDestination(patrolPoints[currentPoint].position);
    }

    void StartActivity()
    {
        // Start performing the activity
        performingActivity = true;

        // Get the activity for the current patrol point
        ActivityType activity = activities[currentPoint];

        // Perform the activity
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

    void EndActivity()
    {
        // End the activity
        performingActivity = false;
        timer = 0f;

        // Select a new patrol point
        SelectRandomPoint();
    }

    void BrushTeeth()
    {
        // Insert code for brushing teeth here
        Debug.Log("Brushing teeth at patrol point " + currentPoint);
    }

    void LookOutWindow()
    {
        // Insert code for looking out window here
        Debug.Log("Looking out window at patrol point " + currentPoint);
    }

    void DoWork()
    {
        // Insert code for looking out window here
        Debug.Log("DoWork at patrol point " + currentPoint);
    }

    void UsePhone()
    {
        // Insert code for looking out window here
        Debug.Log("UsePhone at patrol point " + currentPoint);
    }

    void Dishes()
    {
        // Insert code for looking out window here
        Debug.Log("Dishes at patrol point " + currentPoint);
    }



    IEnumerator WaitBeforeNewActivity()
    {
        yield return new WaitForSeconds(10f);
        EndActivity();
    }

    void DetectPlayer()
    {    
        // Trigger detection consequences
        Debug.Log("Player detected!");
        detectingPlayer = false;
        detectionTimer = 0f;
        detectionBar.SetActive(false);

        // Insert code for triggering alarm or other consequences of being detected
    }

}
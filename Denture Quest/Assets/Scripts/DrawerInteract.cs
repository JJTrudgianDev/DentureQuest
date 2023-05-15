using UnityEngine;
using System.Collections;

public class DrawerInteract : MonoBehaviour
{
    [SerializeField] private GameObject objectToMove;
    [SerializeField] private Transform startPosition;
    [SerializeField] private float transitionTime = 0.5f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private Transform emptyObject;
    [SerializeField] private string cameraName = "Cam";
    [SerializeField] private bool isInteractable = true;


    private Vector3 initialPosition;
    private Coroutine moveCoroutine;
    private bool isLookingAtObject;
    private bool isObjectMoved;
    private Camera raycastCamera;
  

    private void Awake()
    {
        // Find the camera with the specified name
        GameObject cameraObject = GameObject.Find(cameraName);
        if (cameraObject == null)
        {
            Debug.LogError("Could not find camera with name: " + cameraName);
            enabled = false;
            return;
        }

        // Get the Camera component from the camera object
        raycastCamera = cameraObject.GetComponent<Camera>();
        if (raycastCamera == null)
        {
            Debug.LogError("Could not find Camera component on object with name: " + cameraName);
            enabled = false;
            return;
        }

        // Check for null values and set initial position
        if (objectToMove == null || startPosition == null)
        {
            enabled = false;
            return;
        }

        initialPosition = objectToMove.transform.position;
    }

    private void Update()
    {
        if (!isInteractable)
        {
            return;
        }

        bool canMoveObject = isLookingAtObject && Input.GetKeyDown(KeyCode.E);

        if (canMoveObject && !isObjectMoved)
        {
            
            moveCoroutine = StartCoroutine(MoveObject(objectToMove.transform, emptyObject.position, emptyObject.rotation, transitionTime));
            isObjectMoved = true;
        }
        else if (canMoveObject && isObjectMoved)
        {
            
            moveCoroutine = StartCoroutine(MoveObject(objectToMove.transform, startPosition.position, startPosition.rotation, transitionTime));
            isObjectMoved = false;
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(raycastCamera.transform.position, raycastCamera.transform.forward, out hit, detectionRange))
        {
            if (hit.transform.gameObject == objectToMove)
            {
                isLookingAtObject = true;
            }
            else
            {
                isLookingAtObject = false;
            }
        }
        else
        {
            isLookingAtObject = false;
        }
    }

    public IEnumerator MoveObject(Transform objectTransform, Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        if (!isInteractable)
        {
            yield break;
        }

        float elapsedTime = 0f;
        Vector3 startPosition = objectTransform.position;
        Quaternion startRotation = objectTransform.rotation;

        while (elapsedTime < duration)
        {
            objectTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            objectTransform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objectTransform.position = targetPosition;
        objectTransform.rotation = targetRotation;

    }

    public void EnableMoveObject()
    {
        isInteractable = true;
    }
  
    public void DisableMoveObject()
    {
        isInteractable = false;
    }
}

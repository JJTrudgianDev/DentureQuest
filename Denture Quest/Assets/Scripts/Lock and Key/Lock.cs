using UnityEngine;
using UnityEngine.UI;

public class Lock : MonoBehaviour
{
    public string keyTag; // Tag of the corresponding key object
    public float interactionDistance = 2f; // Distance at which the player can interact with the lock

    private bool canInteract = false;
    private Camera playerCamera;
    private RaycastHit hitInfo;

    public RawImage keyUIImage; // Reference to the UI image for the key
    public GameObject objectToDisable; // Reference to the GameObject to disable

    private void Start()
    {
        // Assuming the player has a separate GameObject with a Camera component
        // You can assign the player's camera reference in the Inspector or through code
        playerCamera = Camera.main;
    }

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.F) && IsPlayerInRange())
        {
            AttemptUnlock();
        }
    }

    private void FixedUpdate()
    {
        canInteract = IsLookingAtLock();
    }

    private bool IsLookingAtLock()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hitInfo) && hitInfo.collider.gameObject == gameObject;
    }

    private bool IsPlayerInRange()
    {
        Vector3 playerPosition = playerCamera.transform.position;
        Vector3 lockPosition = transform.position;
        float distance = Vector3.Distance(playerPosition, lockPosition);
        return distance <= interactionDistance;
    }

    private void AttemptUnlock()
    {
        if (InventoryManager.Instance.HasKey(keyTag))
        {
            Unlock();
        }
        else
        {
            WrongKey();
        }
    }

    private void Unlock()
    {
        // Play a sound effect indicating the successful unlock
        // Insert your sound effect code here

        // Shrink the lock object until it reaches a scale of 0 in all dimensions
        StartCoroutine(ShrinkLock());

        // Disable the UI image for the picked up key
        keyUIImage.enabled = false;

        // Disable the referenced object
        objectToDisable.SetActive(false);
    }

    private System.Collections.IEnumerator ShrinkLock()
    {
        while (transform.localScale.magnitude > 0.1)
        {
            transform.localScale -= Vector3.one * Time.deltaTime;
            yield return null;
        }

        // Disable the lock object
        gameObject.SetActive(false);
    }

    private void WrongKey()
    {
        // Play a sound effect indicating the unsuccessful unlock
        // Insert your sound effect code here
    }
}

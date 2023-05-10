using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    public string lockTag; // Tag of the corresponding lock object
    public float interactionDistance = 2f; // Distance at which the player can interact with the key
    public RawImage keyUIImage; // Reference to the UI image for the key

    private bool canInteract = false;
    private Camera playerCamera;
    private RaycastHit hitInfo;

    private void Start()
    {
        // Assuming the player has a separate GameObject with a Camera component
        // You can assign the player's camera reference in the Inspector or through code
        playerCamera = Camera.main;

        keyUIImage.enabled = false;
    }

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.F) && IsPlayerInRange())
        {
            PickUpKey();
        }
    }

    private void FixedUpdate()
    {
        canInteract = IsLookingAtKey();
    }

    private bool IsLookingAtKey()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hitInfo) && hitInfo.collider.gameObject == gameObject;
    }

    private bool IsPlayerInRange()
    {
        Vector3 playerPosition = playerCamera.transform.position;
        Vector3 keyPosition = transform.position;
        float distance = Vector3.Distance(playerPosition, keyPosition);
        return distance <= interactionDistance;
    }

    private void PickUpKey()
    {
        // Disable the key object in the scene
        gameObject.SetActive(false);

        // Enable the UI image for the picked up key
        keyUIImage.enabled = true;

        // Add the key to the inventory manager
        InventoryManager.Instance.AddKey(lockTag, keyUIImage);

        // Play a sound effect indicating the key pickup
        // Insert your sound effect code here
    }
}

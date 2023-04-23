using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public float speed = 6f;
    public float crouchSpeed = 2f;
    public float jumpForce = 10f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public LayerMask groundMask;
    public LayerMask obstacleMask;


    public float smallScale = 0.5f;
    private bool isSmall = false;
    private float originalHeight;
    private Vector3 originalScale;

    public float scaleTime = 0.25f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private ThirdPersonCamera thirdPersonCamera;

    public Transform player;
    public Transform denturesTeleportPosition;

    private bool denturesPickedUp = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        thirdPersonCamera = Camera.main.GetComponent<ThirdPersonCamera>();
        originalHeight = controller.height;
        originalScale = transform.localScale;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Crouch when the button is held down
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!isSmall)
            {
                StartCoroutine(ScalePlayer(originalScale * smallScale, scaleTime));
                isSmall = true;
            }
        }
        // Uncrouch when the button is released and there is enough space above the player's head
        else if (Input.GetKeyUp(KeyCode.LeftControl) && isSmall)
        {
            if (CanUncrouch())
            {
                StartCoroutine(ScalePlayer(originalScale, scaleTime));
                isSmall = false;
            }
        }

        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);

        // Get the movement inputs
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Move the character based on the inputs
        Vector3 moveDirection = thirdPersonCamera.transform.right * horizontal + thirdPersonCamera.transform.forward * vertical;
        moveDirection.y = 0f;
        moveDirection.Normalize();

        // Adjust speed based on whether the character is crouching or not
        float currentSpeed = isSmall ? crouchSpeed : speed;

        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        // Apply gravity to the velocity
        velocity.y += gravity * Time.deltaTime;

        // Apply the velocity to the character controller
        controller.Move(velocity * Time.deltaTime);
    }

    private bool CanUncrouch()
    {
        RaycastHit hit;
        float distance = (originalHeight - originalHeight * smallScale) + 0.1f; // The extra 0.1f is to avoid false collisions due to floating-point errors
        bool canUncrouch = !Physics.Raycast(transform.position, Vector3.up, out hit, distance, obstacleMask);

        return canUncrouch;
    }

    private IEnumerator ScalePlayer(Vector3 targetScale, float duration)
    {
        float timeElapsed = 0;
        Vector3 initialScale = transform.localScale;

        while (timeElapsed < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
    }

    // Teleport Denture Object
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is tagged with "Dentures" and the dentures haven't been picked up yet
        if (other.gameObject.CompareTag("Dentures") && !denturesPickedUp)
        {
            // Set the dentures as picked up
            denturesPickedUp = true;

            // Teleport the dentures to the specified position above the player
            other.transform.position = denturesTeleportPosition.position;
            // Set the player as the parent of the dentures
            other.transform.parent = player;
        }
    }
}



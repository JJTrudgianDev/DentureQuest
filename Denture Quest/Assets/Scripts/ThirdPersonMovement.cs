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

    public float crouchHeight = 0.5f;
    public float crouchRadius = 0.3f; // added
    private bool isCrouching = false;
    private float originalHeight;
    private float originalRadius; // added
    private CharacterController controller;

    private Vector3 velocity;
    private bool isGrounded;

    private ThirdPersonCamera thirdPersonCamera;

    public Transform player;
    public Transform denturesTeleportPosition;

    private bool denturesPickedUp = false;

    private RaycastHit hitInfo; // Added variable for raycast hit detection

    void Start()
    {
        controller = GetComponent<CharacterController>();
        thirdPersonCamera = Camera.main.GetComponent<ThirdPersonCamera>();
        originalHeight = controller.height;
        originalRadius = controller.radius; // added

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Crouch when the button is held down
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!isCrouching)
            {
                StartCoroutine(CrouchPlayer(crouchHeight, crouchRadius)); // modified
                isCrouching = true;
            }
        }
        // Uncrouch when there is enough space above the player's head and the crouch button is not being held down
        else if (!Input.GetKey(KeyCode.LeftControl) && isCrouching && CanUncrouch())
        {
            StartCoroutine(UncrouchPlayer(originalHeight, originalRadius)); // modified
            isCrouching = false;
        }

        // Uncrouch when the button is released and there is enough space above the player's head
        else if (Input.GetKeyUp(KeyCode.LeftControl) && isCrouching)
        {
            if (CanUncrouch())
            {
                StartCoroutine(UncrouchPlayer(originalHeight, originalRadius)); // modified
                isCrouching = false;
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
        float currentSpeed = isCrouching ? crouchSpeed : speed;

        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        // Apply gravity to the velocity
        velocity.y += gravity * Time.deltaTime;

        // Apply the velocity to the character controller
        controller.Move(velocity * Time.deltaTime);

        // Raycast to detect dentures
        if (Physics.Raycast(thirdPersonCamera.transform.position, thirdPersonCamera.transform.forward, out hitInfo, 3f))
        {
            // Check if the raycast hits an object tagged with "Dentures" and the dentures haven't been picked up yet
            if (hitInfo.collider.CompareTag("Dentures") && !denturesPickedUp)
            {
                //            // Show a prompt to pick up dentures, e.g., display a message on the screen

                // Check if the player presses the "F" key
                if (Input.GetKeyDown(KeyCode.F))
                {
                    // Set the dentures as picked up
                    denturesPickedUp = true;

                    // Teleport the dentures to the specified position above the player
                    hitInfo.collider.transform.position = denturesTeleportPosition.position;

                    // Set the player as the parent of the dentures
                    hitInfo.collider.transform.parent = player;
                }
            }
        }
    }

    private bool CanUncrouch()
    {
        RaycastHit hit;
        float distance = originalHeight - crouchHeight + 0.1f; // The extra 0.1f is to avoid false collisions due to floating-point errors
        bool canUncrouch = !Physics.Raycast(transform.position, Vector3.up, out hit, distance, obstacleMask);

        return canUncrouch;
    }

    private IEnumerator CrouchPlayer(float targetHeight, float targetRadius)
    {
        float timeElapsed = 0;
        float initialHeight = controller.height;
        float initialRadius = controller.radius;

        while (timeElapsed < 0.25f)
        {
            controller.height = Mathf.Lerp(initialHeight, targetHeight, timeElapsed / 0.25f);
            controller.radius = Mathf.Lerp(initialRadius, targetRadius, timeElapsed / 0.25f); // added
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        controller.height = targetHeight;
        controller.radius = targetRadius; // added
    }

    private IEnumerator UncrouchPlayer(float targetHeight, float targetRadius)
    {
        float timeElapsed = 0;
        float initialHeight = controller.height;
        float initialRadius = controller.radius;

        while (timeElapsed < 0.25f)
        {
            controller.height = Mathf.Lerp(initialHeight, targetHeight, timeElapsed / 0.25f);
            controller.radius = Mathf.Lerp(initialRadius, targetRadius, timeElapsed / 0.25f); // added
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        controller.height = targetHeight;
        controller.radius = targetRadius; // added
    }
}

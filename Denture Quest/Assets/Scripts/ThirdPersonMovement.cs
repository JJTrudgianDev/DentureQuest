using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public float speed = 6f;
    public float jumpForce = 10f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public LayerMask groundMask;

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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);

        // Get the movement inputs
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Move the character based on the inputs
        Vector3 moveDirection = thirdPersonCamera.transform.right * horizontal + thirdPersonCamera.transform.forward * vertical;
        moveDirection.y = 0f;
        moveDirection.Normalize();
        controller.Move(moveDirection * speed * Time.deltaTime);

        // Jump if the player is grounded and the jump button is pressed
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            
        }

        // Apply gravity to the velocity
        velocity.y += gravity * Time.deltaTime;

        // Apply the velocity to the character controller
        controller.Move(velocity * Time.deltaTime);
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
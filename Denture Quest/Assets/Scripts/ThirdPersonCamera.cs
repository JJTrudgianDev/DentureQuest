using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 5f;
    public float sensitivity = 2f;
    public float smoothSpeed = 10f;
    public Vector2 pitchMinMax = new Vector2(-40f, 85f);
    public LayerMask obstacleLayer; // specify which layer(s) to treat as obstacles

    private float yaw;
    private float pitch;

    void LateUpdate()
    {
        // Get the mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Calculate the target rotation
        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0f);

        // Smoothly rotate the camera towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);

        // Cast a ray from the target to the camera position, to check for obstacles
        RaycastHit hit;
        Vector3 targetPosition = target.position - transform.forward * distance;
        Vector3 direction = targetPosition - target.position;
        float obstacleDistance = distance;
        if (Physics.Raycast(target.position, direction.normalized, out hit, distance, obstacleLayer))
        {
            // If an obstacle is hit, adjust the target position to the hit point
            obstacleDistance = hit.distance;
            targetPosition = hit.point;
        }

        // Move the player character with the camera's rotation
        target.rotation = Quaternion.Euler(0f, yaw, 0f);

        // Smoothly move the camera towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition + transform.forward * obstacleDistance, smoothSpeed * Time.deltaTime);
    }
}

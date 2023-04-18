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

            // Calculate the target position
            Vector3 targetPosition = target.position - transform.forward * distance;

            // Move the player character with the camera's rotation
            target.rotation = Quaternion.Euler(0f, yaw, 0f);

            // Smoothly move the camera towards the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }

    
}

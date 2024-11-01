using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacter : MonoBehaviour
{
   public Transform target;           // The player or target the camera will follow
    public Vector3 offset = new Vector3(0, 5, -10); // Offset from the target position
    public float rotationSpeed = 5f;   // Speed of camera rotation
    public float scrollSpeed = 2f;     // Speed of zoom in/out

    private float _currentZoom = 10f;  // Default zoom level
    private float _pitch = 2f;         // Up/Down angle
    private float _yaw = 0f;           // Left/Right angle

    // New variables for pitch limits
    public float minPitch = -20f;      // Minimum angle to look down
    public float maxPitch = 60f;       // Maximum angle to look up

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraController: Target not assigned.");
            return;
        }

        // Set the initial position based on the offset and zoom
        transform.position = target.position + offset * _currentZoom;
    }

    private void Update()
    {
        HandleRotation();
        HandleZoom();
        UpdateCameraPosition();
    }

    private void HandleRotation()
    {
        // Mouse input for rotation
        _yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        _pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;

        // Clamp the pitch angle to prevent looking too far up or down
        _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);
    }

    private void HandleZoom()
    {
        // Scroll input for zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        _currentZoom -= scroll * scrollSpeed;
        _currentZoom = Mathf.Clamp(_currentZoom, 5f, 15f); // Limit the zoom range
    }

    private void UpdateCameraPosition()
    {
        // Update the camera's position and rotation based on target and input
        Vector3 direction = new Vector3(0, 0, -_currentZoom);
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);

        transform.position = target.position + rotation * offset.normalized * _currentZoom;
        transform.LookAt(target.position + Vector3.up * 1.5f); // Look at the target, with a slight upward offset
    }

}

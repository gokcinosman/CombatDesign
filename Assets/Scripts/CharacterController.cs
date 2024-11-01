using System;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Rigidbody _rb;
    public Transform cameraTransform; // Reference to the camera's transform

    public float rotationSpeed = 10f;
    public float playerSpeed = 2f;
    private float _horizontal;
    private float _vertical;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void MoveCharacter()
    {
        // Adjust movement to be relative to the camera's direction
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Project forward and right vectors onto the XZ plane
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Combine input with camera directions
        Vector3 movement = (forward * _vertical + right * _horizontal).normalized * playerSpeed * Time.deltaTime;
        transform.position += movement;
    }

    private void Update()
    {
        MoveCharacter();
        MakeRotation();
    }

    private void FixedUpdate()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
    }

    private void MakeRotation()
    {
        // Only rotate if there is input
        Vector3 direction = new Vector3(_horizontal, 0, _vertical);
        if (direction.sqrMagnitude > 0.01f)
        {
            Vector3 targetDirection = cameraTransform.TransformDirection(direction);
            targetDirection.y = 0; // Lock rotation to the Y axis
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
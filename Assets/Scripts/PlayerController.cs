using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cameraTransform; // Reference to the camera's transform

    public float rotationSpeed = 10f;
    public float playerSpeed = 2f;
    public float jumpForce = 5f;
    private bool isJumping = false;




    private float _horizontal;
    private Rigidbody _rb;
    private float _vertical;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        // Freeze rotation on X and Z axes to prevent unwanted tilting
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update()
    {
        JumpToDirection();
    }

    private void FixedUpdate()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        if (IsGrounded())
        {
            if (isJumping)
            {
                isJumping = false;
            }
            else if (!isJumping && _rb.velocity.y <= 0.001f)  // Checks for landing
            {
                MakeRotation();
                MoveCharacter();
            }
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(GetBottomPoint(), 0.1f);
    }

    private void MoveCharacter()
    {
        // Adjust movement to be relative to the camera's direction
        var forward = cameraTransform.forward;
        var right = cameraTransform.right;

        // Project forward and right vectors onto the XZ plane
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Combine input with camera directions
        var movement = (forward * _vertical + right * _horizontal).normalized * playerSpeed * Time.deltaTime;
        transform.position += movement;


        if (movement.sqrMagnitude > 0.001f)
        {
            AnimationManager.instance.StartRunning();

        }
        else
        {

            AnimationManager.instance.StopRunning();



        }
    }

    private void JumpToDirection()
    {
        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            isJumping = true;

            AnimationManager.instance.ChangeState(AnimationManager.instance.JUMP);
            // Get the direction based on input
            var jumpDirection = new Vector3(_horizontal, 0, _vertical);

            // If there's no directional input, default to an upward jump
            if (jumpDirection.sqrMagnitude > 0.01f)
            {
                // Adjust direction relative to the camera
                jumpDirection = cameraTransform.TransformDirection(jumpDirection);
                jumpDirection.y = 0;
                jumpDirection.Normalize();
            }
            else
            {
                // Default to straight upward jump if no input
                jumpDirection = Vector3.forward;
                jumpDirection = cameraTransform.TransformDirection(jumpDirection);
                jumpDirection.y = 0;
                jumpDirection.Normalize();
            }


            // Apply the jump force in the computed direction
            _rb.AddForce((jumpDirection * 1 + Vector3.up) * jumpForce, ForceMode.Impulse);
        }
    }


    private bool IsGrounded()
    {
        // Define the radius for the sphere check. Adjust it based on your character's size.
        var groundCheckRadius = 0.1f;
        // Position the ground check at the character's feet.
        var groundCheckPosition = GetBottomPoint();

        // Check if the sphere overlaps with any colliders in the "Ground" layer (LayerMask can be set to specific layers if needed).
        return Physics.CheckSphere(groundCheckPosition, groundCheckRadius, LayerMask.GetMask("Ground"));
    }


    private Vector3 GetBottomPoint()
    {
        Collider col = GetComponent<Collider>();
        return col.bounds.center - new Vector3(0, col.bounds.extents.y, 0);
    }


    private void MakeRotation()
    {
        // Only rotate if there is input
        var direction = new Vector3(_horizontal, 0, _vertical);
        if (direction.sqrMagnitude > 0.01f && IsGrounded())
        {
            var targetDirection = cameraTransform.TransformDirection(direction);
            targetDirection.y = 0; // Lock rotation to the Y axis
            var targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
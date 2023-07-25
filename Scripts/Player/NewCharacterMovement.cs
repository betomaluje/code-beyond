using ECM.Controllers;
using UnityEngine;

public class NewCharacterMovement : BaseCharacterController
{
    [SerializeField] private float turnSmoothTime = 0.01f;
    [SerializeField] private bool rotateWithCamera = true;

    private NewPlayerAnimations playerAnimations;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerAnimations = GetComponent<NewPlayerAnimations>();
    }

    protected override Vector3 CalcDesiredVelocity()
    {
        // in this case we simple move the character's toward its current view direction
        return transform.forward * speed * moveDirection.z;
    }

    protected override void UpdateRotation()
    {
        if (rotateWithCamera)
        {
            // We need to point our player to the cameras forward vector so it's always facing what the camera is looking at
            Vector3 viewDir = cam.transform.forward;

            viewDir.y = 0;
            viewDir.Normalize();

            Quaternion newRot = Quaternion.LookRotation(viewDir);

            // Rotate character along its y-axis (yaw rotation)
            movement.rotation = Quaternion.Slerp(movement.rotation, newRot, turnSmoothTime * Time.deltaTime);
        }

        // Generate a rotation around character's vertical axis (Yaw rotation)
        var rotateAmount = moveDirection.x * angularSpeed * Time.deltaTime;

        // Rotate character along its y-axis (yaw rotation)

        movement.rotation *= Quaternion.Euler(0f, rotateAmount, 0f);
    }

    protected override void Animate()
    {
        if (isJumping && !isGrounded)
        {
            playerAnimations.Jump();
        }

        if (isGrounded)
        {
            if (moveDirection.sqrMagnitude <= 0)
            {
                playerAnimations.Idle();
            }
            else
            {
                playerAnimations.Move();
            }
        }

    }

}

using Beto.Sounds;
using Cinemachine;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private CinemachineFreeLook gameCamera;  // 3rd person freelook camera

    [SerializeField] private float speed = 12f;
    [SerializeField] private float gravityMultiplier = 2f;
    [SerializeField] private float jumpSpeed = 3f;
    [SerializeField] private float turnSmoothTime = 0.01f;

    private Vector3 velocity;
    private bool isGrounded;
    private bool isJumpPressed = false;

    private PlayerAnimations playerAnimations;
    private float turnSmoothVelocity;

    private void Start()
    {
        playerAnimations = GetComponent<PlayerAnimations>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        isGrounded = controller.isGrounded;

        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;

        if (movement.sqrMagnitude > 1f)
        {
            movement.Normalize();
        }

        //playerAnimations.Move(movement.sqrMagnitude);
        playerAnimations.Move();

        if (movement.sqrMagnitude >= 0.05f)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + gameCamera.m_XAxis.Value;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }

        isJumpPressed = Input.GetButtonDown("Jump");

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        playerAnimations.Idle();

        if (isJumpPressed && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        playerAnimations.Jump();
        velocity.y = Mathf.Sqrt(jumpSpeed * Physics.gravity.y * -gravityMultiplier);
        SoundManager.instance.Play("Jump");
    }
}
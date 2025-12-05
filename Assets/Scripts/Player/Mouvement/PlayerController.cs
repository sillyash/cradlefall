using UnityEngine;

public class PlayerController : PlayerMovement
{
    private CharacterController characterController;

    private float speed;

    private Vector3 velocity;
    private Vector3 move;

    void Start()
    {
        KeyMappingSettings.LoadPreset();

        characterController = GetComponent<CharacterController>();
        if (!movementSettings)
        {
            Debug.LogError("Le script CharacterController du prefab Joueur n’a pas de ScriptableObject de mouvement assigné.");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        ApplyGravity();
        ApplyFinalMovement();
    }


    protected override void HandleMovement()
    {
        float axisX = CustomInputRouter.GetAxisHorizontal();
        float axisY = CustomInputRouter.GetAxisVertical();

        speed = Input.GetKey(KeyCode.LeftShift) ? movementSettings.runningSpeed : movementSettings.walkingSpeed;

        // Direction horizontale du transform
        Vector3 forward = transform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = transform.right;
        right.y = 0;
        right.Normalize();

        move = right * axisX + forward * axisY;
    }

    protected override void HandleJump()
    {
        if (characterController.isGrounded && CustomInputRouter.GetButtonJump())
        {
            velocity.y = Mathf.Sqrt(movementSettings.jumpHeight * -2f * PlayerMovementSettings.GRAVITY);
        }
    }

    protected override void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // empêche la gravité de s'accumuler
        }

        velocity.y += PlayerMovementSettings.GRAVITY * Time.deltaTime;
    }

    protected override void ApplyFinalMovement()
    {
        characterController.Move((move * speed + velocity) * Time.deltaTime);
    }
}

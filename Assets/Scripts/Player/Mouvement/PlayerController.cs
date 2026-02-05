using UnityEngine;

public class PlayerController : PlayerMovement
{
    [SerializeField] private Camera mainCamera;

    private CharacterController characterController;

    private float speed;

    private Vector3 velocity;
    private Vector3 move;
    private Animator animator;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        KeyMappingSettings.LoadPreset();

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (!movementSettings)
        {
            Debug.LogError("Le script CharacterController du prefab Joueur n’a pas de ScriptableObject de mouvement assigné.");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (!IsOwner) return;

        HandleMovement();
        HandleJump();
        ApplyGravity();
        ApplyFinalMovement();
        updateAnim();
    }


    protected override void HandleMovement()
    {
        float axisX = CustomInputRouter.GetAxisHorizontal();
        float axisY = CustomInputRouter.GetAxisVertical();

        speed = Input.GetKey(KeyCode.LeftShift) ? movementSettings.runningSpeed : movementSettings.walkingSpeed;

        // Direction horizontale du transform
        Vector3 forward = mainCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = mainCamera.transform.right;
        right.y = 0;
        right.Normalize();

        move = right * axisX + forward * axisY;
    }

    public void updateAnim()
    {

        animator.SetBool("onGround", characterController.isGrounded);
        animator.SetFloat("movingSpeed", move.magnitude * speed);
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

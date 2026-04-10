using UnityEngine;

public class PlayerController : PlayerMovement
{
    [Header("Volumes personnalisés")]
    [SerializeField, Range(0f, 1f)] private float jumpVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float sprintVolume = 1f;
    
    [Header("Sound FX Manager")]
    [SerializeField] private SoundFXManager soundFXManager;

    [SerializeField] private Camera mainCamera;

    private CharacterController characterController;
    private float speed;
    private Vector3 velocity;
    private Vector3 move;
    private Animator animator;

    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip sprintSound;
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private float footstepInterval = 0.35f; // base si besoin
    [SerializeField] [Range(0f,1f)] private float footstepVolume = 0.6f;

    private float footstepTimer;
    private bool wasSprinting;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        KeyMappingSettings.LoadPreset();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        if (!movementSettings)
        {
            Debug.LogError("Le script CharacterController du prefab Joueur n�a pas de ScriptableObject de mouvement assign�.");
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
        applySpeedFX();
        HandleFootsteps();
    }
    
    protected override void HandleMovement()
    {
        float axisX = CustomInputRouter.GetAxisHorizontal();
        float axisY = CustomInputRouter.GetAxisVertical();

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        speed = isSprinting ? movementSettings.runningSpeed : movementSettings.walkingSpeed;

        // Son de sprint uniquement au début du sprint ET si le joueur bouge
        bool isMoving = Mathf.Abs(axisX) > 0.1f || Mathf.Abs(axisY) > 0.1f;
        if (isSprinting && !wasSprinting && isMoving)
        {
            if (sprintSound != null && soundFXManager != null)
                soundFXManager.PlaySoundFXClip(sprintSound, transform, sprintVolume);
        }
        wasSprinting = isSprinting;

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
            if (jumpSound != null && soundFXManager != null)
            {
                soundFXManager.PlaySoundFXClip(jumpSound, transform, jumpVolume);
            }
        }
    }

    public void applySpeedFX()
    {
        float targetFOV = 80;
        
        if (speed > movementSettings.walkingSpeed)
        {
            targetFOV = 90;
        }

        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, 0.1f);
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

    private void HandleFootsteps()
    {
        bool isWalking = characterController.isGrounded && move.magnitude > 0.1f;
        if (isWalking)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                // Plus rapide en courant
                float interval = footstepInterval;
                if (Input.GetKey(KeyCode.LeftShift))
                    interval *= 0.8f;
                footstepTimer = interval;
                if (footstepSounds != null && footstepSounds.Length > 0 && soundFXManager != null)
                {
                    var clip = footstepSounds[UnityEngine.Random.Range(0, footstepSounds.Length)];
                    soundFXManager.PlaySoundFXClip(clip, transform, footstepVolume);
                }
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }
}

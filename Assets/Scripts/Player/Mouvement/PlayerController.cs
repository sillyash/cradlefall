using UnityEngine;

public class PlayerController : PlayerMovement
{
    private CharacterController characterController;

    private float speed;

    private Vector3 velocity;
    private Vector3 move;

    void Start()
    {
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

        // --- TODO Debug TO REMOVE ---
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Dessine le raycast en vert pendant 1 seconde
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green, 1f);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Found " + hit.collider.gameObject + " at distance: " + hit.distance);

                baseEnemies enemy = hit.collider.GetComponent<baseEnemies>();
                if (enemy != null)
                    enemy.TakeDammage(5);
            }
            else
            {
                Debug.Log("Nothing found");
            }
        }
    }


    protected override void HandleMovement()
    {
        float axisX = Input.GetAxis("Horizontal");
        float axisY = Input.GetAxis("Vertical");

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
        if (characterController.isGrounded && Input.GetButtonDown("Jump"))
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

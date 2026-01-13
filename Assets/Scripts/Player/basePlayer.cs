using Unity.Netcode;
using UnityEngine;

public class basePlayer : NetworkBehaviour, IDamageable, ICanDie
{
    public static float TARGET_DISTANCE = 5;

    [SerializeField] private BaseWeapon weapon;
    public BaseWeapon CurrentWeapon => weapon; // Pour affichage HUD
    public float health { get; set; }
    public float maxHealth { get; set; }

    [SerializeField] private HealthBarScript healthBar;

    [SerializeField] private Camera mainCamera;

    public bool isDead { get; set; } = false;

    public PlayerStats stats;

    private Vector3 targetPoint;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        if (!stats)
        {
            Debug.LogError($"Le Joueur {this.name} n'a pas SO stats");
            enabled = false;
            return;
        }

        health = stats.health;
        maxHealth = stats.health;
    }

    void Update()
    {
        if (!IsOwner) return;
        raycast();
        weapon.transform.LookAt(targetPoint);


        weaponInput();
    }

    void weaponInput()
    {
        if (CustomInputRouter.GetButtonReload())
            
        {
            weapon.Reload();
        }

        else

        if (CustomInputRouter.GetButtonShoot())
        {
            weapon.Fire();
        }
    }

    Vector3 calcTargetPoint()
    {
        return mainCamera.transform.position + mainCamera.transform.forward * TARGET_DISTANCE;
    }

    void raycast()
    {
        // When cursor is locked (gameplay), raycast from screen center (camera forward)
        // When cursor is unlocked (menu/unfocused), use calculated forward point
        Ray ray;
        
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            // In gameplay: raycast from center of screen
            ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        }
        else
        {
            // Cursor visible: use forward projection instead of mouse position to avoid flipping
            targetPoint = calcTargetPoint();
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Ignore projectile
            if(!hit.collider.TryGetComponent(out ProjectileBase var))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = calcTargetPoint();
            }
        }
        else
        {
            targetPoint = calcTargetPoint();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            isDead = true;
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Joueur Mort");
        GameManager.instance.GameOver();
    }
}

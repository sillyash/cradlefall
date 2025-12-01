using UnityEngine;

public class basePlayer : MonoBehaviour
{
    public static float TARGET_DISTANCE = 5;

    [SerializeField] private BaseWeapon weapon;

    public PlayerStats stats;
    [SerializeField] private float health;

    private Vector3 targetPoint;

    void Start()
    {
        if (!stats)
        {
            Debug.LogError($"Le Joueur {this.name} n'a pas SO stats");
            enabled = false;
            return;
        }

        health = stats.health;
    }

    public void TakeDamage(float damage)
    {
        raycast();
        weapon.transform.LookAt(targetPoint);


        weaponInput();
    }

    void weaponInput()
    {
        if (Input.GetButtonDown("Reload"))
        {
            weapon.Reload();
        }

        else

        if (Input.GetButton("Fire1"))
        {
            weapon.Fire();
        }
    }

    Vector3 calcTargetPoint()
    {
        return Camera.main.transform.position + Camera.main.transform.forward * TARGET_DISTANCE;
    }

    void raycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Ignore projectile
            if(!hit.collider.TryGetComponent(out ProjectileBase var))
            {
                targetPoint = hit.point;
                weapon.transform.LookAt(targetPoint);
            }
        } else
        {
            targetPoint = calcTargetPoint();
        }
    }
}

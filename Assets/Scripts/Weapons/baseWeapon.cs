using UnityEngine;
using System.Collections;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField] private WeaponBehavior behavior;
    [SerializeField] private WeaponStats stats;
    [SerializeField] private ProjectileBase projectile;
    
    private int ammoCount;
    private bool isReadyToShoot = true;


    void Start()
    {
        ammoCount = stats.magSize;
    }

    void Update()
    {
        if (isReadyToShoot)
        {
            Fire();
        }
    }

    public void Fire()
    {
        behavior.OnFire(projectile, transform.position);
        ammoCount--;
        isReadyToShoot = false;
        StartCoroutine(Cooldown(stats.shootingCooldown));
        isReadyToShoot = (ammoCount > 0);
    }

    IEnumerator Cooldown(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public void Reload()
    {
        StartCoroutine(Cooldown(stats.reloadTime));
        ammoCount = stats.magSize;
    }
}
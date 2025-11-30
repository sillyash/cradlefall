using UnityEngine;
using System.Collections;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField] private WeaponBehavior behavior;
    [SerializeField] private WeaponStats stats;
    [SerializeField] private ProjectileBase projectile;
    [SerializeField] private GameObject projectileSpawn;
    
    private int ammoCount;
    private bool isReadyToShoot = true;


    void Start()
    {
        ammoCount = stats.magSize;
    }


    // Testing purpose
    //void Update()
    //{
    //    Fire();
    //}

    public void Fire()
    {
        if (isReadyToShoot && ammoCount > 0) {
            behavior.OnFire(projectile, projectileSpawn.transform.transform, stats.spread);
            ammoCount--;

            isReadyToShoot = false;
            StartCoroutine(FireCooldown());
        }  
    }

    IEnumerator FireCooldown()
    {
        yield return new WaitForSeconds(stats.shootingCooldown);
        isReadyToShoot = true;
    }

    public void Reload()
    {
        Debug.Log("reloading...");
        StartCoroutine(ReloadCooldown());
    }


    public IEnumerator ReloadCooldown()
    {
        yield return new WaitForSeconds(stats.reloadTime);
        ammoCount = stats.magSize;
        Debug.Log("Done");
    }
}
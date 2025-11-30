using UnityEngine;
using System.Collections;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField] private WeaponBehavior behavior;
    [SerializeField] private WeaponStats stats;
    [SerializeField] private ProjectileBase projectile;
    
    private int ammoCount;
    private bool isReadyToShoot = true;
    private bool isOnCooldown = false;


    void Start()
    {
        ammoCount = stats.magSize;
    }


    // Testing purpose
    ///*void Update()
    //{
    //    Fire();
    //}*/

    public void Fire()
    {
        if (isReadyToShoot) {
        behavior.OnFire(projectile, transform);
        ammoCount--;

        isReadyToShoot = false;
        StartCoroutine(FireCooldown());
        }  
    }

    IEnumerator FireCooldown()
    {
        yield return new WaitForSeconds(stats.shootingCooldown);
        isReadyToShoot = (ammoCount > 0);
    }




    public IEnumerator ReloadCooldown()
    {
        yield return new WaitForSeconds(stats.reloadTime);
        ammoCount = stats.magSize;
        Debug.Log("Done");
    }
}

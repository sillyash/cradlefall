using UnityEngine;
using System.Collections;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField] private WeaponBehavior behavior;
    [SerializeField] private WeaponStats stats;

    public int ammoCount;
    public bool isReadyToShoot = true;

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
        behavior.OnFire();
        ammoCount--;
        isReadyToShoot = false;
        StartCoroutine(Cooldown(stats.shootingCooldown));
        isReadyToShoot = true;
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
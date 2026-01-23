using UnityEngine;
using System.Collections;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField] public WeaponBehavior behavior;
    [SerializeField] public WeaponStats stats;
    [SerializeField] public ProjectileBase projectile;
    [SerializeField] public GameObject projectileSpawn;
    
    private int ammoCount;
    private bool isReadyToShoot = true;


    // Variables partager a l'HUD munitions
    public int AmmoCount => ammoCount;
    public int MagSize => stats.magSize;
    public event System.Action<BaseWeapon> OnFire;
    public event System.Action OnAmmoChanged;
    public event System.Action OnReloadStart;
    public event System.Action OnReloadEnd;


    void Start()
    {
        ammoCount = stats.magSize;
        OnAmmoChanged?.Invoke();
        StartCoroutine(WaitForInstance());
    }

    private IEnumerator WaitForInstance()
    {
        yield return new WaitUntil(() => ProjectileVisualSync.instance != null);
        ProjectileVisualSync.instance.subToWeaponFire(this);
    }

    // Testing purpose
    //void Update()
    //{
    //    Fire();
    //}

    public void Fire()
    {
        if (isReadyToShoot && ammoCount > 0) {
            Quaternion projRot = projectileSpawn.transform.rotation;

            behavior.OnFire(projectile, projectileSpawn.transform.transform, stats.spread);
            ammoCount--;
            OnAmmoChanged?.Invoke();
            OnFire?.Invoke(this);

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
        OnReloadStart?.Invoke();
    }


    public IEnumerator ReloadCooldown()
    {
        yield return new WaitForSeconds(stats.reloadTime);
        ammoCount = stats.magSize;
        OnAmmoChanged?.Invoke();
        OnReloadEnd?.Invoke();
        Debug.Log("Done");
    }
}

using UnityEngine;
using System.Collections;

public class BaseWeapon : MonoBehaviour
{

    [Header("Sound FX Manager")]
    [SerializeField] private SoundFXManager soundFXManager;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] [Range(0f, 1f)] private float fireSoundVolume = 1f;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] [Range(0f, 1f)] private float reloadSoundVolume = 1f;
    [SerializeField] public WeaponBehavior behavior;
    [SerializeField] public WeaponStats stats;
    [SerializeField] public ProjectileBase projectile;
    [SerializeField] public GameObject projectileSpawn;
    
    private MuzzleFlash muzzleFlash;
    
    private int ammoCount;
    private bool isReadyToShoot = true;
    private bool isReloading = false;

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
        muzzleFlash = GetComponent<MuzzleFlash>();
        StartCoroutine(WaitForInstance());
    }

    private IEnumerator WaitForInstance()
    {
        yield return new WaitUntil(() => ProjectileVisualSync.instance != null);
        ProjectileVisualSync.instance.subToWeaponFire(this);
    }

    public void Fire()
    {
        if (!isReloading && isReadyToShoot && ammoCount > 0) {
            Quaternion projRot = projectileSpawn.transform.rotation;

            behavior.OnFire(projectile, projectileSpawn.transform.transform, stats.spread);
            ammoCount--;
            OnAmmoChanged?.Invoke();
            OnFire?.Invoke(this);
            if (muzzleFlash != null) // Only for weapons with muzzle flash
            {
                muzzleFlash.PlayMuzzleFlash();
            }
            if (fireSound != null && soundFXManager != null)
            {
                soundFXManager.PlaySoundFXClip(fireSound, transform, fireSoundVolume);
            }
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
        if (isReloading || ammoCount == stats.magSize)
        {
            return;
        }

        isReloading = true;
        StartCoroutine(ReloadCooldown());
        OnReloadStart?.Invoke();
        if (reloadSound != null && soundFXManager != null)
        {
            soundFXManager.PlaySoundFXClip(reloadSound, transform, reloadSoundVolume);
        }
    }


    public IEnumerator ReloadCooldown()
    {
        yield return new WaitForSeconds(stats.reloadTime);
        ammoCount = stats.magSize;
        isReloading = false;
        OnAmmoChanged?.Invoke();
        OnReloadEnd?.Invoke();
    }
}

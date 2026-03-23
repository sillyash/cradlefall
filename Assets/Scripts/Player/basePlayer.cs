using System;
using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static UnityEngine.GraphicsBuffer;
using Constants;

public class basePlayer : NetworkBehaviour, IDamageable, ICanDie
{
    
    [Header("Sound FX Manager")]
    [SerializeField] private SoundFXManager soundFXManager;
    public static float TARGET_DISTANCE = 5;
    public static float MIN_RAY_DISTANCE = 3;

    public Roles.Role role;

    [SerializeField] private GameObject weaponPrefab;

    // Rig and IK
    [SerializeField] private RigBuilder rigBuilder;
    [SerializeField] private TwoBoneIKConstraint rightHandIK;
    [SerializeField] private TwoBoneIKConstraint leftHandIK;

    private BaseWeapon weapon;
    public PlayerBattery battery { get; private set; }

    public float health { get; set; }
    public float maxHealth { get; set; }

    [Header("Sons vie")]
    [SerializeField] private AudioClip[] damageSounds;
    [SerializeField, Range(0f, 1f)] private float damageSoundVolume = 1f;
    [SerializeField] private AudioClip healSound;
    [SerializeField] private AudioClip lowHealthSound;
    [SerializeField, Range(0.01f, 1f)] private float lowHealthThreshold = 0.25f;
    [SerializeField] private float lowHealthRepeatDelay = 5f;
    [SerializeField] private float damageSoundCooldown = 0.1f;
    [SerializeField] private float healSoundCooldown = 0.25f;

    private float lastHealth = 0f;
    private float lastDamageSoundTime = -100f;
    private float lastHealSoundTime = -100f;
    private float lastLowHealthSoundTime = -100f;
    private bool lowHealthActive = false;

    [SerializeField] private HealthBarScript healthBar;
    [SerializeField] private DamageUI damageUI;
    [SerializeField] public Camera mainCamera;
    [SerializeField] private Transform FPSWeaponAnchorPoint;
    [SerializeField] private GameObject playerCanvas;

    public bool isDead { get; set; } = false;

    public PlayerStats stats;

    private Vector3 targetPoint;
    

    public override void OnNetworkSpawn()
    {
        if (!stats)
        {
            Debug.LogError($"[Player] Le Joueur {this.name} n'a pas SO stats");
            enabled = false;
            return;
        }

        battery = GetComponent<PlayerBattery>();

        if (IsServer)
        {
            var weaponObj = Instantiate(weaponPrefab, FPSWeaponAnchorPoint);
            var weaponNetObj = weaponObj.GetComponent<NetworkObject>();

            weaponNetObj.SpawnWithOwnership(OwnerClientId);
            weaponNetObj.TrySetParent(GetComponent<NetworkObject>());
        }

        if (IsOwner)
        {
            Debug.Log($"[Player] Owner instanciate {OwnerClientId}");
            var weaponObjOwner = Instantiate(weaponPrefab, FPSWeaponAnchorPoint);
            weapon = weaponObjOwner.GetComponent<BaseWeapon>();
            weaponObjOwner.transform.SetParent(FPSWeaponAnchorPoint);

            if (weapon.TryGetComponent<NetworkObject>(out var netObj))
            {
                Destroy(netObj);
            }

            // Stick hands to weapon
            setHandTarget();

            health = stats.health;
            maxHealth = stats.health;
            lastHealth = health;
            lowHealthActive = health <= maxHealth * lowHealthThreshold;
            NetworkObject.DestroyWithScene = false;
        }
        else
        {
            NetworkObject.DestroyWithScene = true;
            Destroy(playerCanvas); // Do not display other player's HUD
        }
    }

    void setHandTarget()
    {
        // Find by name, not perfect but couldn't find a better way
        Transform handTargetR = weapon.transform.Find("Hand_R_target");
        Transform handTargetL = weapon.transform.Find("Hand_L_target");

        rightHandIK.data.target = handTargetR;
        rightHandIK.data.hint = handTargetR.GetChild(0);

        leftHandIK.data.target = handTargetL;
        leftHandIK.data.hint = handTargetL.GetChild(0);

        rigBuilder.Build();
    }

    void Update()
    {
        if (!IsOwner) return;
        if (PauseMenu.isPaused) return;

        if (!Mathf.Approximately(health, lastHealth))
        {
            HandleHealthChanged(lastHealth, health);
            lastHealth = health;
        }

        raycast();

        setWeaponRotation(45f, 10f);

        weaponInput();
    }

    void setWeaponRotation(float maxY, float maxX)
    {
        weapon.transform.LookAt(targetPoint);

        Vector3 localAngle = weapon.transform.localEulerAngles;

        localAngle.y = Mathf.Clamp(localAngle.y, 360f - maxY, 360f);
        localAngle.x = Mathf.Clamp(localAngle.x, 360f - maxX, 360f);

        weapon.transform.localEulerAngles = localAngle;
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
            if (weapon.AmmoCount > 0)
            {
                weapon.Fire();
            } else
            {
                weapon.Reload();
            }
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
            if(!hit.collider.TryGetComponent(out ProjectileBase var) && isNotDamageable(hit))
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


    public bool isNotDamageable(RaycastHit hit) 
    {
        return !(hit.collider.TryGetComponent(out IDamageable hitObject) && hit.distance < MIN_RAY_DISTANCE);
    }

    private void HandleHealthChanged(float oldHealth, float newHealth)
    {
        if (!IsOwner || Mathf.Approximately(oldHealth, newHealth) || soundFXManager == null) return;

        float now = Time.time;

        bool wasLow = maxHealth > 0 && oldHealth <= maxHealth * lowHealthThreshold;
        bool isLow = maxHealth > 0 && newHealth <= maxHealth * lowHealthThreshold;

        if (newHealth < oldHealth && damageSounds != null && damageSounds.Length > 0 && now - lastDamageSoundTime >= damageSoundCooldown)
        {
            var clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
            if (clip != null)
            {
                soundFXManager.PlaySoundFXClip(clip, transform, damageSoundVolume);
                damageUI.alphaTarget = 1f;
                lastDamageSoundTime = now;
            }
        }

        if (newHealth > oldHealth && healSound != null && now - lastHealSoundTime >= healSoundCooldown)
        {
            soundFXManager.PlaySoundFXClip(healSound, transform, 1f);
            lastHealSoundTime = now;
        }

        if (isLow)
        {
            if (!lowHealthActive || now - lastLowHealthSoundTime >= lowHealthRepeatDelay)
            {
                if (lowHealthSound != null)
                {
                    soundFXManager.PlaySoundFXClip(lowHealthSound, transform, 1f);
                }
                lowHealthActive = true;
                lastLowHealthSoundTime = now;
            }
        }
        else
        {
            lowHealthActive = false;
        }
    }

    public void Heal(float amount)
    {
        if (!IsOwner || isDead || amount <= 0f) return;

        float oldHealth = health;
        health = Mathf.Min(maxHealth, health + amount);
        HandleHealthChanged(oldHealth, health);
    }


    public void TakeDamage(float damage)
    {
        if (!IsOwner) return;

        float oldHealth = health;
        health = Mathf.Max(0f, health - damage);

        HandleHealthChanged(oldHealth, health);

        if (health <= 0f)
        {
            isDead = true;
            Die();
        }
    }

    public void Die()
    {
        Debug.LogWarning("[Player] Joueur Mort");
        
        // Notify server/PlayerManager
        if (IsServer)
        {
            PlayerManager.Instance?.OnPlayerDeath(OwnerClientId);
        }
        else
        {
            NotifyDeathRpc();
        }
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void NotifyDeathRpc()
    {
        if (IsServer)
        {
            PlayerManager.Instance?.OnPlayerDeath(OwnerClientId);
        }
    }

    public void DestroyOnNetwork()
    {
        Die();
    }

    public ulong getClientId()
    {
        return OwnerClientId;
    }

    public BaseWeapon CurrentWeapon => weapon; // Pour affichage HUD
}

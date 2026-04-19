using System;
using System.Collections;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering;
using TMPro;
using static UnityEngine.GraphicsBuffer;
using Constants;

using Random = UnityEngine.Random;

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

    //Screen shake
    private Vector3 camLocalPos;
    private float shakeStartTime = -10f; //Set to negative value to disable it by default 

    [SerializeField] private HealthBarScript healthBar;
    [SerializeField] private DamageUI damageUI;
    [SerializeField] public Camera mainCamera;
    [SerializeField] private Transform FPSWeaponAnchorPoint;
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private TMP_Text playerTagText;
    [SerializeField] private Vector3 playerTagOffset = new Vector3(0f, 2.2f, 0f);
    [SerializeField] private bool hideTagForOwner = true;

    private NetworkVariable<FixedString64Bytes> networkPlayerTag = new(
        default,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public bool isDead { get; set; } = false;

    public string PlayerTag => networkPlayerTag.Value.ToString();

    public PlayerStats stats;

    private Vector3 targetPoint;
    

    public override void OnNetworkSpawn()
    {
        networkPlayerTag.OnValueChanged += OnNetworkPlayerTagChanged;

        if (!IsOwner)
        {
            EnsureWorldPlayerTag();
            RefreshPlayerTagVisual(networkPlayerTag.Value.ToString());
        }

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
            camLocalPos = mainCamera.transform.localPosition;

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

    public override void OnNetworkDespawn()
    {
        networkPlayerTag.OnValueChanged -= OnNetworkPlayerTagChanged;

        if (playerTagText != null && playerTagText is TextMeshPro && !IsHudTag(playerTagText))
        {
            Destroy(playerTagText.gameObject);
            playerTagText = null;
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

        screenShake(0.5f, 0.2f);
    }

    void screenShake(float duration, float amplitude)
    {
        if (Time.fixedTime - shakeStartTime < duration)
        {
            Vector3 shake = Random.insideUnitSphere;
            shake.z = 0;
            mainCamera.transform.localPosition = camLocalPos + shake * amplitude;
        }
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

        float now = Time.fixedTime;

        bool wasLow = maxHealth > 0 && oldHealth <= maxHealth * lowHealthThreshold;
        bool isLow = maxHealth > 0 && newHealth <= maxHealth * lowHealthThreshold;

        if (newHealth < oldHealth && damageSounds != null && damageSounds.Length > 0 && now - lastDamageSoundTime >= damageSoundCooldown)
        {
            var clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
            if (clip != null)
            {
                soundFXManager.PlaySoundFXClip(clip, transform, damageSoundVolume);
                damageUI.alphaTarget = 1f - newHealth/maxHealth;
                shakeStartTime = now;
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
                    damageUI.minTarget = 0.1f;
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

    public void SetPlayerTagServer(string tag)
    {
        if (!IsServer)
            return;

        string sanitizedTag = SanitizeTag(tag);
        Debug.Log($"[Player] Server setting player tag for {OwnerClientId} to '{sanitizedTag}'.");
        networkPlayerTag.Value = new FixedString64Bytes(sanitizedTag);
    }

    private void OnNetworkPlayerTagChanged(FixedString64Bytes previousTag, FixedString64Bytes newTag)
    {
        if (IsOwner)
            return;

        Debug.Log($"[Player] Network player tag changed for {OwnerClientId}: '{previousTag}' -> '{newTag}'.");
        RefreshPlayerTagVisual(newTag.ToString());
    }

    private void RefreshPlayerTagVisual(string tag)
    {
        if (IsOwner)
            return;

        Debug.Log($"[Player] Applying player tag visual for {OwnerClientId}: '{tag}'.");
        EnsureWorldPlayerTag();

        if (playerTagText != null)
        {
            playerTagText.gameObject.SetActive(true);
            playerTagText.text = tag;
        }
    }

    private static string SanitizeTag(string rawTag)
    {
        if (string.IsNullOrWhiteSpace(rawTag))
            return "Player";

        string trimmed = rawTag.Trim();
        return trimmed.Length > 16 ? trimmed.Substring(0, 16) : trimmed;
    }

    private void EnsureWorldPlayerTag()
    {
        if (IsOwner)
            return;

        if (playerTagText != null)
        {
            bool isTextMeshPro3D = playerTagText is TextMeshPro;
            bool isOnPlayerHud = IsHudTag(playerTagText);

            if (isTextMeshPro3D && !isOnPlayerHud)
            {
                ConfigureTagRendering(playerTagText);
                return;
            }
        }

        GameObject tagObject = new GameObject("PlayerTag", typeof(TextMeshPro), typeof(FollowCamera));
        Transform tagTransform = tagObject.transform;
        tagTransform.SetParent(transform, false);
        tagTransform.localPosition = playerTagOffset;
        tagTransform.localRotation = Quaternion.identity;
        tagTransform.localScale = Vector3.one * 0.1f;

        TextMeshPro tmp = tagObject.GetComponent<TextMeshPro>();
        tmp.text = string.Empty;
        tmp.alignment = TextAlignmentOptions.Center;
        
        // Auto font size
        tmp.enableAutoSizing = true;
        tmp.fontSizeMin = 1f;

        // Color
        tmp.color = Color.white;
        tmp.outlineWidth = 0.2f;
        tmp.outlineColor = Color.black;

        FollowCamera follow = tagObject.GetComponent<FollowCamera>();
        if (follow != null)
            follow.SetLockXRotation(false);

        ConfigureTagRendering(tmp);

        playerTagText = tmp;
        tagObject.SetActive(true);
    }

    private void ConfigureTagRendering(TMP_Text text)
    {
        if (text == null)
            return;

        Material sourceMaterial = text.fontSharedMaterial != null ? text.fontSharedMaterial : text.fontMaterial;
        if (sourceMaterial != null)
        {
            Shader overlayShader = Shader.Find("TextMeshPro/Distance Field Overlay");
            Material overlayMaterial;

            if (overlayShader != null)
            {
                overlayMaterial = new Material(overlayShader);
                overlayMaterial.CopyPropertiesFromMaterial(sourceMaterial);
            }
            else
            {
                overlayMaterial = new Material(sourceMaterial);
            }

            overlayMaterial.name = sourceMaterial.name + "_PlayerTagOverlay";

            if (overlayMaterial.HasProperty("_ZTestMode"))
                overlayMaterial.SetFloat("_ZTestMode", (float)CompareFunction.Always);
            else if (overlayMaterial.HasProperty("_ZTest"))
                overlayMaterial.SetFloat("_ZTest", (float)CompareFunction.Always);

            if (overlayMaterial.HasProperty("_ZWrite"))
                overlayMaterial.SetFloat("_ZWrite", 0f);

            // Keep tags readable in dark scenes by forcing bright face + glow when supported.
            if (overlayMaterial.HasProperty("_FaceColor"))
                overlayMaterial.SetColor("_FaceColor", new Color(2f, 2f, 2f, 1f));

            if (overlayMaterial.HasProperty("_GlowColor"))
                overlayMaterial.SetColor("_GlowColor", new Color(1f, 1f, 1f, 1f));

            if (overlayMaterial.HasProperty("_GlowPower"))
                overlayMaterial.SetFloat("_GlowPower", 0.6f);

            if (overlayMaterial.HasProperty("_GlowOuter"))
                overlayMaterial.SetFloat("_GlowOuter", 0.25f);

            overlayMaterial.renderQueue = (int)RenderQueue.Overlay;
            overlayMaterial.EnableKeyword("GLOW_ON");
            text.fontSharedMaterial = overlayMaterial;
        }

        TextMeshPro text3D = text as TextMeshPro;
        if (text3D == null)
            return;

        text3D.isOverlay = true;

        Renderer textRenderer = text3D.GetComponent<Renderer>();
        if (textRenderer == null)
            return;

        textRenderer.shadowCastingMode = ShadowCastingMode.Off;
        textRenderer.receiveShadows = false;
        textRenderer.sortingOrder = short.MaxValue;

        text.color = Color.white;
    }

    private bool IsHudTag(TMP_Text text)
    {
        return playerCanvas != null && text != null && text.transform.IsChildOf(playerCanvas.transform);
    }
}

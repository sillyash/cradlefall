using UnityEngine;
using TMPro;
using DG.Tweening;


public class AmmoDisplayScript : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoDisplay;
    [SerializeField] private basePlayer player;

    private BaseWeapon weapon;
    private Color originalColor;

    // Backup Inspector Valeurs
    private Color defaultFaceColor;
    private Color defaultOutlineColor;
    private float defaultOutlineWidth;

    private Color defaultUnderlayColor;
    private Vector2 defaultUnderlayOffset;
    private float defaultUnderlaySoftness;

    private Material ammoMat; // Style police

    // Gestion cooldown empty mag feedback
    private float lastEmptyTime = 0f;
    private float emptyCooldown = 0.2f;

    private void Awake()
    {
        ammoMat = Instantiate(ammoDisplay.fontMaterial);
        ammoDisplay.fontMaterial = ammoMat;

        CacheDefaultValues();
    }

    private void CacheDefaultValues()
    {
        defaultFaceColor = ammoMat.GetColor("_FaceColor");

        defaultOutlineColor = ammoMat.GetColor("_OutlineColor");
        defaultOutlineWidth = ammoMat.GetFloat("_OutlineWidth");

        defaultUnderlayColor = ammoMat.GetColor("_UnderlayColor");
        defaultUnderlayOffset = new Vector2(
            ammoMat.GetFloat("_UnderlayOffsetX"),
            ammoMat.GetFloat("_UnderlayOffsetY")
        );
        defaultUnderlaySoftness = ammoMat.GetFloat("_UnderlaySoftness");
    }



    private void Start()
    {
        if (ammoDisplay == null)
        {

            ammoDisplay = GetComponent<TMP_Text>();
            if (ammoDisplay == null)
            {
                Debug.LogError("AmmoDisplayScript : aucun TMP_Text trouvé sur cet objet !");
                return;
            }
        }

        originalColor = ammoDisplay.color;

        if (player == null)
        {
            Debug.LogError("AmmoDisplayScript : aucun Player assigné !");
            return;
        }

        // Récupérer l'arme actuelle du Player
        weapon = player.CurrentWeapon;
        if (weapon == null)
        {
            Debug.LogWarning("AmmoDisplayScript : le Player n'a pas d'arme équipée !");
            return;
        }

        // Actualisation de l'affichage 
        weapon.OnAmmoChanged += UpdateAmmoDisplay;
        weapon.OnFire += AmmoDisplayAnimation;

        // Mettre à jour l'affichage dès le départ
        UpdateAmmoDisplay();
    }

    private void UpdateAmmoDisplay()
    {
        if (weapon == null || ammoDisplay == null) return;

        RestoreDefaultVisuals();
        ammoDisplay.text = $"{weapon.AmmoCount}";
        RestoreDefaultVisuals();
    }

    private void Update()
    {
        if (weapon == null) return;

        // Tir à vide → animation empty ammo
        if (weapon.AmmoCount == 0 && CustomInputRouter.GetButtonShoot())
        {
            if (Time.time - lastEmptyTime > emptyCooldown)
            {
                PlayEmptyAmmoFeedback();
                lastEmptyTime = Time.time;
            }
        }
    }


    private void AmmoDisplayAnimation(BaseWeapon wep)
    {
        if (ammoDisplay == null) return;

        // Si ammo == 0, ne rien faire ici (handled par Update())
        if (weapon.AmmoCount == 0) return;

        // Stoppe toutes les animations précédentes pour éviter accumulation
        ammoDisplay.transform.DOKill();
        ammoDisplay.DOKill();

        // 1️⃣ Shake léger en position (pas de scale)
        Vector3 originalPos = ammoDisplay.transform.localPosition;
        ammoDisplay.transform.DOShakePosition(0.1f, strength: 8f, vibrato: 14, randomness: 100, fadeOut: true)
            .OnComplete(() => ammoDisplay.transform.localPosition = originalPos);

        // 2️⃣ Flash blanc très rapide
        ammoDisplay.DOColor(Color.white, 0.05f).OnComplete(() =>
        {
            ammoDisplay.DOColor(originalColor, 0.15f);
        });

        if (weapon.AmmoCount <= 5)
        {
            ApplyLowAmmoStyle();
        }
        else
        {
            // >5 balles → retour normal
            RestoreDefaultVisuals();
        }

        // --- ANIMATION EMPTY AMMO ---
        if (weapon.AmmoCount == 0)
        {
            // Ne touche pas au style, juste animation
            PlayEmptyAmmoFeedback();
        }
    }

    // Low ammo discret
    private void ApplyLowAmmoStyle()
    {
        ammoMat.SetColor("_FaceColor", Color.white);
        ammoMat.SetColor("_OutlineColor", new Color(0.5f, 0.05f, 0.05f)); // rouge plus foncé/discret
        ammoMat.SetFloat("_OutlineWidth", defaultOutlineWidth * 0.8f);     // outline plus fine

        ammoMat.SetColor("_UnderlayColor", new Color(0.5f, 0.05f, 0.05f, 0.6f)); // ombre plus légère
        ammoMat.SetFloat("_UnderlayOffsetX", 0.5f);
        ammoMat.SetFloat("_UnderlayOffsetY", -0.3f);
        ammoMat.SetFloat("_UnderlaySoftness", 0.3f);
    }

    // Empty ammo voyant
    private void ApplyEmptyAmmoStyle()
    {
        ammoMat.SetColor("_FaceColor", Color.white);
        ammoMat.SetColor("_OutlineColor", new Color(0.55f, 0.05f, 0.05f)); // un peu plus foncé
        ammoMat.SetFloat("_OutlineWidth", defaultOutlineWidth * 1.5f);

        ammoMat.SetColor("_UnderlayColor", new Color(0.55f, 0.05f, 0.05f, 0.8f));
        ammoMat.SetFloat("_UnderlayOffsetX", 0.8f);
        ammoMat.SetFloat("_UnderlayOffsetY", 0f);
        ammoMat.SetFloat("_UnderlaySoftness", 0.6f);
    }


    public void PlayEmptyAmmoFeedback()
    {
        ammoDisplay.transform.DOKill();
        Vector3 originalPos = ammoDisplay.transform.localPosition;

        // Shake + pulse
        ammoDisplay.transform
            .DOShakePosition(0.10f, strength: 5f, vibrato: 10, randomness: 50, fadeOut: true)
            .OnComplete(() => ammoDisplay.transform.localPosition = originalPos);

        ammoDisplay.transform
            .DOScale(1f, 0.08f)
            .SetLoops(2, LoopType.Yoyo)
            .OnKill(() => ammoDisplay.transform.localScale = Vector3.one);

        // Appliquer style empty ammo
        ApplyEmptyAmmoStyle();

        // Retour au style low ammo si ammo >0 et ≤5 après 0.2s
        DOVirtual.DelayedCall(0.2f, () =>
        {
            if (weapon != null && weapon.AmmoCount > 0 && weapon.AmmoCount <= 5)
                ApplyLowAmmoStyle();
            else if (weapon != null && weapon.AmmoCount > 5)
                RestoreDefaultVisuals();

            ammoDisplay.transform.localScale = Vector3.one;
        });
    }



    private void RestoreDefaultVisuals()
    {
        ammoMat.SetColor("_FaceColor", defaultFaceColor);

        ammoMat.SetColor("_OutlineColor", defaultOutlineColor);
        ammoMat.SetFloat("_OutlineWidth", defaultOutlineWidth);

        ammoMat.SetColor("_UnderlayColor", defaultUnderlayColor);
        ammoMat.SetFloat("_UnderlayOffsetX", defaultUnderlayOffset.x);
        ammoMat.SetFloat("_UnderlayOffsetY", defaultUnderlayOffset.y);
        ammoMat.SetFloat("_UnderlaySoftness", defaultUnderlaySoftness);
    }

    private void OnDestroy()
    {
        if (weapon != null)
        {
            weapon.OnAmmoChanged -= UpdateAmmoDisplay;
            weapon.OnFire -= AmmoDisplayAnimation;
        }
    }
}

using UnityEngine;
using TMPro;
using DG.Tweening;


public class AmmoDisplayScript : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoDisplay;
    [SerializeField] private basePlayer player;

    private BaseWeapon weapon;
    private Color originalColor;

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

        ammoDisplay.text = $"{weapon.AmmoCount}";
    }

    private void AmmoDisplayAnimation()
    {
        if (ammoDisplay == null) return;

        // Stoppe toutes les animations précédentes pour éviter accumulation
        ammoDisplay.transform.DOKill();
        ammoDisplay.DOKill();

        // 1️⃣ Shake léger en position (pas de scale)
        Vector3 originalPos = ammoDisplay.transform.localPosition;
        ammoDisplay.transform.DOShakePosition(0.1f, strength: 5f, vibrato: 10, randomness: 90, fadeOut: true)
            .OnComplete(() => ammoDisplay.transform.localPosition = originalPos);

        // 2️⃣ Flash blanc très rapide
        ammoDisplay.DOColor(Color.white, 0.05f).OnComplete(() =>
        {
            ammoDisplay.DOColor(originalColor, 0.15f);
        });
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

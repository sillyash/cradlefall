using UnityEngine;
using TMPro;

public class MagSizeScript : MonoBehaviour
{
    [SerializeField] private TMP_Text magSizeDisplay;
    [SerializeField] private basePlayer player;

    private BaseWeapon weapon;

    private void Start()
    {
        if (magSizeDisplay == null)
        {

            magSizeDisplay = GetComponent<TMP_Text>();
            if (magSizeDisplay == null)
            {
                Debug.LogError("MagSizeDisplayScript : aucun TMP_Text trouv  sur cet objet !");
                return;
            }
        }

        if (player == null)
        {
            Debug.LogError("MagSizeDisplayScript : aucun Player assign  !");
            return;
        }

        // R cup rer l'arme actuelle du Player
        weapon = player.CurrentWeapon;
        if (weapon == null)
        {
            Debug.LogWarning("MagSizeDisplayScript : le Player n'a pas d'arme  quip e !");
            return;
        }
        UpdateAmmoDisplay();
    }

    private void UpdateAmmoDisplay()
    {
        if (weapon == null || magSizeDisplay == null) return;

        magSizeDisplay.text = $"{weapon.MagSize}";
    }

    private void OnDestroy()
    {
        if (weapon != null)
            weapon.OnAmmoChanged -= UpdateAmmoDisplay;
    }
}

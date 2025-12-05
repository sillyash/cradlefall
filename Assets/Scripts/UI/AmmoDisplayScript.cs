using UnityEngine;
using TMPro;

public class AmmoDisplayScript : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoDisplay; 
    [SerializeField] private basePlayer player;   

    private BaseWeapon weapon;

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

        // Mettre à jour l'affichage dès le départ
        UpdateAmmoDisplay();
    }

    private void UpdateAmmoDisplay()
    {
        if (weapon == null || ammoDisplay == null) return;

        ammoDisplay.text = $"{weapon.AmmoCount} / {weapon.MagSize}";
    }

    private void OnDestroy()
    {
        if (weapon != null)
            weapon.OnAmmoChanged -= UpdateAmmoDisplay;
    }
}

using UnityEngine;
using TMPro;

public class ReloadingDisplayScript : MonoBehaviour
{
    [SerializeField] private TMP_Text reloadText;
    [SerializeField] private basePlayer player;

    private BaseWeapon weapon;

    // Appel  par ton WeaponManager ou basePlayer lorsqu'une nouvelle arme est  quip e
    public void Start()
    {
        if (reloadText == null)
        {

            reloadText = GetComponent<TMP_Text>();
            if (reloadText == null)
            {
                Debug.LogError("ReloadingDisplayScript : aucun TMP_Text trouv  sur cet objet !");
                return;
            }
        }

        if (player == null)
        {
            Debug.LogError("ReloadingDisplayScript : aucun Player assign  !");
            return;
        }

        weapon = player.CurrentWeapon;
        if (weapon == null)
        {
            Debug.LogWarning("MagSizeDisplayScript : le Player n'a pas d'arme  quip e !");
            return;
        }

        weapon.OnReloadStart += ShowReloadIcon;
        weapon.OnReloadEnd += HideReloadIcon;

        HideReloadIcon();

    }

    private void ShowReloadIcon()
    {
        reloadText.text = "R";
    }

    private void HideReloadIcon()
    {
        reloadText.text = "";
    }
}


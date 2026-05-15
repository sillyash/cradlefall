using Constants;
using UnityEngine;
using UnityEngine.UI;

public class RoleModuleScript : MonoBehaviour
{
    [Header("Role Images")]
    [SerializeField] private Image dpsImage;
    [SerializeField] private Image techImage;
    [SerializeField] private Image healImage;
    [SerializeField] private Image tankImage;

    private bool _initialized = false;

    void Update()
    {
        if (_initialized) return;

        // On attend que le joueur local soit spawnerr
        foreach (var player in FindObjectsByType<basePlayer>(FindObjectsSortMode.None))
        {
            if (!player.IsOwner) continue;

            ApplyRole(player.role);
            _initialized = true;
            break;
        }
    }

    private void ApplyRole(Roles.Role role)
    {
        dpsImage?.gameObject.SetActive(false);
        techImage?.gameObject.SetActive(false);
        healImage?.gameObject.SetActive(false);
        tankImage?.gameObject.SetActive(false);

        switch (role)
        {
            case Roles.Role.DPS: dpsImage?.gameObject.SetActive(true); break;
            case Roles.Role.Technicien: techImage?.gameObject.SetActive(true); break;
            case Roles.Role.Soigneur: healImage?.gameObject.SetActive(true); break;
            case Roles.Role.Tank: tankImage?.gameObject.SetActive(true); break;
            default:
                Debug.LogWarning($"[RoleIconHUD] Rôle inconnu : {role}");
                break;
        }
    }
}
using System.Collections.Generic;
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

    [Header("Border")]
    [SerializeField] private Image borderImage;

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color brokenColor = Color.red;

    private bool _initialized = false;
    private Image _activeImage;

    void Update()
    {
        if (_initialized) return;
        if (RoleModuleManager.instance == null) return;

        foreach (var player in FindObjectsByType<basePlayer>(FindObjectsSortMode.None))
        {
            if (!player.IsOwner) continue;

            Init(player.role);
            _initialized = true;
            break;
        }
    }

    private void Init(Roles.Role role)
    {
        // Affiche la bonne icône
        dpsImage?.gameObject.SetActive(false);
        techImage?.gameObject.SetActive(false);
        healImage?.gameObject.SetActive(false);
        tankImage?.gameObject.SetActive(false);

        switch (role)
        {
            case Roles.Role.DPS: _activeImage = dpsImage; dpsImage?.gameObject.SetActive(true); break;
            case Roles.Role.Technicien: _activeImage = techImage; techImage?.gameObject.SetActive(true); break;
            case Roles.Role.Soigneur: _activeImage = healImage; healImage?.gameObject.SetActive(true); break;
            case Roles.Role.Tank: _activeImage = tankImage; tankImage?.gameObject.SetActive(true); break;
            default:
                Debug.LogWarning($"[RoleModuleScript] Rôle inconnu : {role}");
                return;
        }

        // Trouve le bon module et s'abonne aux events
        BaseModule linkedModule = role switch
        {
            Roles.Role.DPS => RoleModuleManager.instance.DPSModule,
            Roles.Role.Technicien => RoleModuleManager.instance.TechnicienModule,
            Roles.Role.Soigneur => RoleModuleManager.instance.SoigneurModule,
            Roles.Role.Tank => RoleModuleManager.instance.TankModule,
            _ => null
        };

        if (linkedModule == null)
        {
            Debug.LogWarning($"[RoleModuleScript] Module introuvable pour le rôle {role}");
            return;
        }

        // Abonnement au NetworkVariable du bon module uniquement
        linkedModule.broken.OnValueChanged += (_, isBroken) => SetBrokenColor(isBroken);

        // État initial
        SetBrokenColor(linkedModule.broken.Value);

    }

    private void SetBrokenColor(bool isBroken)
    {
        Color target = isBroken ? brokenColor : normalColor;

        if (_activeImage != null)
            _activeImage.color = target;

        if (borderImage != null)
            borderImage.color = target;
    }
}
using UnityEngine;
using Unity.Netcode;
using Constants;
public class InteractionManager : NetworkBehaviour
{
    public static InteractionManager Instance { get; private set; }

    private basePlayer player;
    public Camera playerCamera;

    // Timer interne
    private float holdTimer = 0f;

    // Pour éviter de rejouer le son à chaque frame
    private bool hasPlayedRepairSound = false;

    // ====== DONN�ES PUBLIQUES POUR L'UI ======
    public float HoldTimer => holdTimer;                 // Temps actuel maintenu
    public float CurrentHoldDuration { get; private set; } // Temps requis pour l'action
    public bool IsHoldingInteract { get; private set; }    // Bouton E maintenu ou non

    [SerializeField] private float healPerSecond = 4f;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void Start()
    {
        if (!IsOwner) return;
        player = GetComponent<basePlayer>();
    }


    private void Update()
    {
        if (!IsOwner) return;
        IsHoldingInteract = CustomInputRouter.GetButtonInteract();
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, player.stats.InteractionRange))
        {
            GameObject hitObject = hit.transform.gameObject;

            if (IsHoldingInteract)
            {
                HandleInteraction(hitObject);
            }
            else
            {
                ResetHold();
            }
        }
        else
        {
            ResetHold();
        }
    }

    private void HandleInteraction(GameObject target)
    {
        if (!IsOwner) return;
        // ===== MODULE =====
        BaseModule module = target.GetComponent<BaseModule>();
        if (module != null)
        {
            if (module.behaviour is RoleModuleBehaviour)
            {
                // Si c'est un module de rôle on vérif si on a le bon rôle
                RoleModuleBehaviour behaviour = (RoleModuleBehaviour) module.behaviour;
                if (behaviour.role != player.role) return;
            }
            if (!module.broken.Value)
            {
                ResetHold();
                return;
            }
            CurrentHoldDuration = module.stats.repairTime;
            holdTimer += Time.deltaTime;
            if (player.role == Roles.Role.Technicien) 
                CurrentHoldDuration = CurrentHoldDuration/2 ;

            // Joue le son de réparation au début du maintien
            if (!hasPlayedRepairSound && holdTimer > 0f)
            {
                module.PlayRepairStartSound();
                hasPlayedRepairSound = true;
            }

            if (holdTimer >= CurrentHoldDuration)
            {
                module.RepairServerRpc();
                ResetHold();
            }

            return;
        }

        // ===== CAPSULE =====
        CryogenicBox capsule = target.GetComponent<CryogenicBox>();
        if (capsule != null)
        {
            // Pas de maintien, soin instantan� progressif
            CurrentHoldDuration = 0f;
            capsule.Heal(Time.deltaTime * healPerSecond);
            holdTimer = 0f;
            return;
        }

        ResetHold();
    }

    private void ResetHold()
    {
        // Si un module est en cours d'interaction, coupe le son de réparation
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, player.stats.InteractionRange))
        {
            BaseModule module = hit.transform.GetComponent<BaseModule>();
            if (module != null)
            {
                module.StopRepairSound();
            }
        }
        holdTimer = 0f;
        CurrentHoldDuration = 0f;
        hasPlayedRepairSound = false;
    }
}

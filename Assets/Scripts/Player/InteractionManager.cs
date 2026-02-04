using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    private basePlayer player;

    // Timer interne
    private float holdTimer = 0f;

    // ====== DONNÉES PUBLIQUES POUR L'UI ======
    public float HoldTimer => holdTimer;                 // Temps actuel maintenu
    public float CurrentHoldDuration { get; private set; } // Temps requis pour l'action
    public bool IsHoldingInteract { get; private set; }    // Bouton E maintenu ou non

    [SerializeField] private float healPerSecond = 4f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        player = GetComponent<basePlayer>();
    }

    private void Update()
    {
        IsHoldingInteract = CustomInputRouter.GetButtonInteract();

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
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
        // ===== MODULE =====
        BaseModule module = target.GetComponent<BaseModule>();
        if (module != null)
        {
            if (!module.broken)
            {
                ResetHold();
                return;
            }

            CurrentHoldDuration = module.stats.repairTime;
            holdTimer += Time.deltaTime;

            if (holdTimer >= CurrentHoldDuration)
            {
                module.Repair();
                ResetHold();
            }

            return;
        }

        // ===== CAPSULE =====
        CryogenicBox capsule = target.GetComponent<CryogenicBox>();
        if (capsule != null)
        {
            // Pas de maintien, soin instantané progressif
            CurrentHoldDuration = 0f;
            capsule.Heal(Time.deltaTime * healPerSecond);
            holdTimer = 0f;
            return;
        }

        ResetHold();
    }

    private void ResetHold()
    {
        holdTimer = 0f;
        CurrentHoldDuration = 0f;
    }
}

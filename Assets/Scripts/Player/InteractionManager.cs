using UnityEngine;
using Unity.Netcode;

public class InteractionManager : NetworkBehaviour
{
    public static InteractionManager Instance { get; set; }
    private basePlayer player;

    private float holdTimer = 0f;

    [SerializeField]private float healPerSeconde = 4;

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

        player = GetComponent<basePlayer>();
    }

    private void Update()
    {

        if (CustomInputRouter.GetButtonInteract())
        {

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, player.stats.InteractionRange))
            {
                GameObject objectHitByRaycast = hit.transform.gameObject;

                if (objectHitByRaycast.GetComponent<BaseModule>() != null)
                // interaction avec un module
                {
                    BaseModule module = objectHitByRaycast.GetComponent<BaseModule>();

                    if (!module.broken.Value) return;
                    {
                        holdTimer += Time.deltaTime;

                        if (holdTimer >= module.stats.repairTime)
                        {
                            module.RepairServerRpc();
                            holdTimer = 0f;
                        }
                    }
                }
                if (objectHitByRaycast.GetComponent<CryogenicBox>() != null)
                // interaction avec une capsule
                {
                    CryogenicBox capsule = objectHitByRaycast.GetComponent<CryogenicBox>();
                    
                    capsule.Heal(Time.deltaTime*healPerSeconde);
                    
                }
            }
        }
        else
        {
            holdTimer = 0f;
        }
    }
}

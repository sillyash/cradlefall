using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }
    private basePlayer player;

    private float holdTimer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        player = GetComponent<basePlayer>();
    }

    private void Update()
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

                if (!module.broken) return;
                if (Input.GetKey(KeyCode.E))
                {
                    holdTimer += Time.deltaTime;

                    if (holdTimer >= module.stats.repairTime)
                    {
                        module.Repair();
                        holdTimer = 0f;
                    }
                }
                else
                {
                    holdTimer = 0f;
                }
            }
        }
    }
}

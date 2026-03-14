using Unity.Netcode;
using UnityEngine;

public class HealingZone : NetworkBehaviour
{
    [SerializeField] private float healPerSeconde = 3;

    void OnTriggerStay(Collider other)
    {
        if (!IsServer) return;

        if (other.TryGetComponent(out basePlayer player))
        {
            if (player.role != Constants.Roles.Role.Soigneur)
            {
                player.Heal(healPerSeconde * Time.deltaTime);
            }
        }
    }
}

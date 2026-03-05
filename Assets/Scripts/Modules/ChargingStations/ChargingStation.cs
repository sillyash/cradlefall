using Unity.Netcode;
using UnityEngine;

public class ChargingStation : NetworkBehaviour
{
    [SerializeField] private float batteryPerSeconde = 3;
    public NetworkVariable<bool> Broken = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    void OnTriggerStay(Collider other)
    {
        if (!IsServer) return;
        if (Broken.Value) return;

        if (other.TryGetComponent(out basePlayer player))
        {
            player.battery.Recharge(batteryPerSeconde * Time.deltaTime);
        }
    }
}

using Unity.Netcode;
using UnityEngine;

public class PlayerBattery : NetworkBehaviour
{
    [SerializeField] protected PlayerStats stats;

    public NetworkVariable<float> BatteryLevel = new(
        100f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public override void OnNetworkSpawn()
    {
        if (stats == null) { Debug.Log($"Player's {name} Battery has no stats"); }
        if (!IsServer) return;
        BatteryLevel.Value = stats.maxBattery;
    }

    public bool IsEmpty()
    {
        return (BatteryLevel.Value == 0);
    }

    public bool HasEnough(float ammount)
    {
        return (BatteryLevel.Value >= ammount);
    }

    public void Consume(float amount)
    {
        if (!IsOwner) return;
        ConsumeBatteryServerRpc(amount);
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void ConsumeBatteryServerRpc(float amount)
    {
        if (!IsServer) return;
        BatteryLevel.Value = Mathf.Max(0, BatteryLevel.Value - amount);
    }

    public void Recharge(float amount)
    {
        if (!IsServer) return;
        float wantedLevel = BatteryLevel.Value + amount;
        float ammount = Mathf.Min(wantedLevel, stats.maxBattery);
        BatteryLevel.Value = ammount;
    }

    public float getPourcentOfBattery()
    {
        return ((BatteryLevel.Value * 100) / stats.maxBattery);
    }
}

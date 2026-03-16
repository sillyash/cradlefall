using Unity.Netcode;
using UnityEngine;

public abstract class VisionDevice : NetworkBehaviour, IBatteryUser
{
    public float BatteryUsage { get; set; }

    public NetworkVariable<bool> IsActive = new(
        false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private PlayerBattery BatteryManager;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        BatteryManager = GetComponent<PlayerBattery>();

        IsActive.OnValueChanged += (oldValue, newValue) =>
        {
            if (newValue)
                OnActivate();
            else
                OnDeactivate();
        };
    }

    public virtual void Update()
    {
        if (!IsOwner) return;
        if (!BatteryManager.IsEmpty())
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                UpdateIsActiveServerRpc(!IsActive.Value);
            }
        }
        else
        {
            if (IsActive.Value)
            {
                UpdateIsActiveServerRpc(false);
            }
        }

        if (IsActive.Value)
        {
            UseBattery(BatteryUsage);
        }
    }

    public virtual void UseBattery(float ammount)
    {
        if (!IsOwner) return;
        BatteryManager.Consume(ammount * Time.deltaTime);
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void UpdateIsActiveServerRpc(bool active)
    {
        if (!IsServer) return;
        IsActive.Value = active;
    }

    public abstract void OnActivate();
    public abstract void OnDeactivate();
}

using Unity.Netcode;
using UnityEngine;

public class CryogenicBox : NetworkBehaviour, IDamageable
{
    public float maxHealth { get; set; } = 500;

    public NetworkVariable<float> Health = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<bool> Broken = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public float health
    {
        get => Health.Value;
        set { if (IsServer) Health.Value = value; }
    }

    public bool broken
    {
        get => Broken.Value;
        set { if (IsServer) Broken.Value = value; }
    }

    public override void OnNetworkSpawn()
    {
        NetworkObject.DestroyWithScene = true;
        if (!IsServer) return;
        Health.Value = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (Broken.Value) return;
        if (!IsServer) return;

        Health.Value -= amount;

        if (Health.Value <= 0)
        {
            Break();
        }
    }

    public void Break()
    {
        if (Broken.Value) return;
        Broken.Value = true;
        GameManager.instance.GameOver();
    }

    public void Heal(float ammount)
    {
        HealRpc(ammount);
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]

    public void HealRpc(float ammount)
    {
        if (Health.Value < (0.75 * maxHealth))
        {
            Health.Value += ammount;
        }
    }

    public bool IsBroken()
    {
        return Broken.Value;
    }

    public float Gethealth()
    {
        return Health.Value;
    }
}
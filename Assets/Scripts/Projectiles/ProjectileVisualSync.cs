using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class ProjectileVisualSync : NetworkBehaviour
{
    public static ProjectileVisualSync instance = null;
    private static List<BaseWeapon> weaponList = new();

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OnNetworkDespawn()
    {
        foreach (BaseWeapon weapon in weaponList)
        {
            weapon.OnFire -= OnWeaponFire;
        }
        base.OnNetworkDespawn();
    }

    public void subToWeaponFire(BaseWeapon weapon)
    {
        weapon.OnFire += OnWeaponFire;
        weaponList.Add(weapon);
    }

    private void OnWeaponFire(BaseWeapon weapon)
    {
        ulong sender = weapon.GetComponentInParent<basePlayer>().getClientId();
        var targetClients = NetworkManager.Singleton.ConnectedClientsIds
                    .Where(id => id != sender)
                    .ToArray();
        
        if (targetClients.Length > 0)
        {
            FireRpc(weaponList.IndexOf(weapon), RpcTarget.Group(targetClients, RpcTargetUse.Temp));
        }
    }

    [Rpc(SendTo.SpecifiedInParams)]
    private void FireRpc(int weaponIndex, RpcParams rpcParams = default)
    {
        BaseWeapon weapon = ProjectileVisualSync.weaponList[weaponIndex];
        Quaternion spreadedRotation = weapon.behavior.CalcSpread(weapon.projectileSpawn.transform.transform, weapon.stats.spread);
        ProjectileBase projectile = Instantiate(weapon.projectile, weapon.transform.position, spreadedRotation);
        projectile.gameObject.tag = "VisualOnly";
    }
}

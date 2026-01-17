using Unity.Netcode;
using UnityEngine;

public class TPSWeaponSync : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            gameObject.SetActive(false);
        }
        else
        {
            transform.localRotation = Quaternion.identity;
        }
    }
}

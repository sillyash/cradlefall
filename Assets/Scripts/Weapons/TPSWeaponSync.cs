using Unity.Netcode;
using UnityEngine;

public class TPSWeaponSync : NetworkBehaviour
{
    public void Awake()
    {
        if (IsOwner)
        {
            gameObject.SetActive(false);
        }
        else
        {
            transform.localRotation = Quaternion.identity;
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            gameObject.SetActive(false);
        }
    }
}

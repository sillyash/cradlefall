using UnityEngine;
using Unity.Netcode;

public class CameraController : NetworkBehaviour
{
    public GameObject cameraHolder;
    public override void OnNetworkSpawn()
    {
        cameraHolder.SetActive(IsOwner);
    }
}

using UnityEngine;

public class LightTorch : VisionDevice
{
    [SerializeField]  private GameObject torchLight;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        torchLight.SetActive(false);
        base.OnNetworkSpawn();
        BatteryUsage = 4;
    }


    public override void OnActivate()
    {
        if (!IsOwner) return;
        torchLight.SetActive(true);
    }

    public override void OnDeactivate()
    {
        if (!IsOwner) return;
        torchLight.SetActive(false);
    }
}

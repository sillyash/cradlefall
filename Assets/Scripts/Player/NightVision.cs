using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class NightVision : VisionDevice
{
    public Volume nightVolume;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        nightVolume.weight = 0f;
        base.OnNetworkSpawn();
        BatteryUsage = 2;
    }

    public override void OnActivate()
    {
        if (!IsOwner) return;
        nightVolume.weight = IsActive.Value ? 1f : 0f;
    }

    public override void OnDeactivate()
    {
        if (!IsOwner) return;
        nightVolume.weight = 0f;
    }
}
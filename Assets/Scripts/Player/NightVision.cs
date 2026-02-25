using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class NightVision : VisionDevice
{
    public Volume nightVolume;

    public override void Start()
    {
        nightVolume.weight = 0f;
        base.Start();
        BatteryUsage = 2;
    }

    public override void OnActivate()
    {
        nightVolume.weight = IsActive ? 1f : 0f;
    }

    public override void OnDeactivate()
    {
        nightVolume.weight = 0f;
    }
}
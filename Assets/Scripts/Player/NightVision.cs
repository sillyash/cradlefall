using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class NightVision : VisionDevice
{
    public Volume nightVolume;
    private bool nightActive = false;

    public override void Start()
    {
        nightVolume.weight = 0f;
        base.Start();
    }

    public override void OnActivate()
    {
        nightVolume.weight = nightActive ? 1f : 0f;
    }

    public override void OnDeactivate()
    {
        nightVolume.weight = 0f;
    }
}
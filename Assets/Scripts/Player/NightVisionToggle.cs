using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class NightVisionToggle : MonoBehaviour
{
    public Volume nightVolume;
    private bool nightActive = false;

    void Start()
    {
        nightVolume.weight = 0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            nightActive = !nightActive;
            nightVolume.weight = nightActive ? 1f : 0f;
        }
    }
}
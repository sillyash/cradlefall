using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class NightVision : MonoBehaviour, IBatteryUser
{
    public Volume nightVolume;
    private bool nightActive = false;
    private PlayerBattery BatteryManager;

    public float BatteryUsage { get; set; } = 2; // per second

    void Start()
    {
        nightVolume.weight = 0f;
        BatteryManager = GetComponent<PlayerBattery>();
    }

    void Update()
    {
        if (!BatteryManager.IsEmpty())
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                nightActive = !nightActive;
                nightVolume.weight = nightActive ? 1f : 0f;
            }
        }
        else
        {
            nightActive = false;
            nightVolume.weight = 0f;
        }

        if (nightActive)
        {
            UseBattery(BatteryUsage);
        }
    }

    public void UseBattery(float ammount)
    {
        BatteryManager.Consume(ammount * Time.deltaTime);
    }
}
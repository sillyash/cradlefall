using UnityEngine;

public interface IBatteryUser
{
    float BatteryUsage { get; set; }
    void UseBattery(float ammount);
}

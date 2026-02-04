using UnityEngine;

public class PlayerBattery : MonoBehaviour
{
    [SerializeField] protected PlayerStats stats;
    public float BatteryLevel { get; private set; }

    public void Start()
    {
        if (stats == null) { Debug.Log($"Player's {name} Battery has no stats"); }
        BatteryLevel = stats.maxBattery;
    }

    public bool IsEmpty()
    {
        return (BatteryLevel == 0);
    }

    public bool HasEnough(float ammount)
    {
        return (BatteryLevel >= ammount);
    }

    public void Consume(float amount)
    {
        float wantedLevel = BatteryLevel - amount;
        BatteryLevel = Mathf.Max(0, wantedLevel);
    }

    public void Recharge(float amount)
    {
        float wantedLevel = BatteryLevel + amount;
        BatteryLevel = Mathf.Min(0, stats.maxBattery);
    }

    public float getPourcentOfBattery()
    {
        return ((BatteryLevel * 100) / stats.maxBattery);
    }
}

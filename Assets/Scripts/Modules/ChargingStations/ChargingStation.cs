using UnityEngine;

public class ChargingStation : MonoBehaviour
{
    [SerializeField] private float batteryPerSeconde = 3;
    public bool Broken = false;
    void OnTriggerStay(Collider other)
    {
        if (Broken) return;
        if (other.TryGetComponent(out basePlayer player))
        {
            player.battery.Recharge(batteryPerSeconde * Time.deltaTime);
        }
    }

    public void Break() 
    {
        Broken = true;
    }
}

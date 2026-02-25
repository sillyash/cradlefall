using UnityEngine;

public class ChargingStation : MonoBehaviour
{
    [SerializeField] private float batteryPerSeconde = 3;
    void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out basePlayer player))
        {
            player.battery.Recharge(batteryPerSeconde * Time.deltaTime);
        }
    }
}

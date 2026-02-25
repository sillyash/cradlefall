using UnityEngine;

public abstract class VisionDevice : MonoBehaviour, IBatteryUser
{
    public float BatteryUsage { get; set; }
    public bool IsActive = false;

    private PlayerBattery BatteryManager;

    public virtual void Start()
    {
        BatteryManager = GetComponent<PlayerBattery>();
    }

    public virtual void Update()
    {
        if (!BatteryManager.IsEmpty())
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                IsActive = !IsActive;
                if (IsActive)
                {
                    OnActivate();
                }
                else
                {
                    OnDeactivate();
                }
            }
        }
        else
        {
            if (IsActive)
            {
                IsActive = false;
                OnDeactivate();
            }
        }

        if (IsActive)
        {
            UseBattery(BatteryUsage);
        }
    }

    public virtual void UseBattery(float ammount)
    {
        BatteryManager.Consume(ammount * Time.deltaTime);
    }

    public abstract void OnActivate();
    public abstract void OnDeactivate();
}

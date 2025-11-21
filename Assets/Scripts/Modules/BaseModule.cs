using UnityEngine;

public class BaseModule : MonoBehaviour
{
    [SerializeField] private ModuleBehavior behaviour;
    private bool broken = false;

    public void Break()
    {
        if (broken) return;
        broken = true;
        behaviour.OnBreak(this);
    }

    public void Repair()
    {
        if (!broken) return;
        broken = false;
        behaviour.OnRepair(this);
    }
}

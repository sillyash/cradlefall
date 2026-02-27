using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Modules/Behaviours/Role")]
public class RoleModuleBehaviour : ModuleBehavior
{
    BaseModule module;
    public event System.Action<BaseModule> OnBreakEvent;
    public event System.Action<BaseModule> OnRepairEvent;

    public bool isBroken = false;

    public override void OnBreak(BaseModule module)
    {
        isBroken = true;
        OnBreakEvent?.Invoke(module);
    }

    public override void OnRepair(BaseModule module)
    {
        isBroken = false;
        OnRepairEvent?.Invoke(module);
    }
}

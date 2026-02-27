using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Modules/Behaviours/Role")]
public class RoleModuleBehaviour : ModuleBehavior
{
    BaseModule module;

    public bool isBroken = false;

    public override void OnBreak(BaseModule module)
    {
        isBroken = true;
    }

    public override void OnRepair(BaseModule module)
    {
        isBroken = false;
    }
}

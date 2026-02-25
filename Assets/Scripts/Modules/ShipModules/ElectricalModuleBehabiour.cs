using UnityEngine;

[CreateAssetMenu(menuName = "Modules/Behaviours/Electrical")]
public class ElectricalModuleBehabiour : ModuleBehavior
{
    public override void OnBreak(BaseModule module)
    {
        LightManager.instance.TurnOffEveryLights();
    }

    public override void OnRepair(BaseModule module)
    {
        LightManager.instance.TurnOnEveryLights();
    }
}

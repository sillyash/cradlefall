using UnityEngine;

[CreateAssetMenu(menuName = "Modules/Behaviours/Electrical")]
public class ElectricalModuleBehabiour : ModuleBehavior
{
    public override void OnBreak(BaseModule module)
    {
        LightManager.instance.TurnOffEveryLights();
        ChargingStationManager.instance.TurnOffAllStations();
    }

    public override void OnRepair(BaseModule module)
    {
        LightManager.instance.TurnOnEveryLights();
        ChargingStationManager.instance.TurnOnAllStations();
    }
}

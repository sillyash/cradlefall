using UnityEngine;

/// <summary>
/// Classe Abstraite pour les comportement des modules.
/// Gère les fonctions OnBreak et OnRepair.
/// </summary>
public abstract class ModuleBehavior : ScriptableObject
{
    public virtual void OnStart(BaseModule module) { return; }

    // A voir si on a vraiment de passer de Module - peut-être inutile
    public abstract void OnBreak(BaseModule module);
    public abstract void OnRepair(BaseModule module);
}

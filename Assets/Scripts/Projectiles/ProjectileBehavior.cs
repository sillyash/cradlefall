using UnityEngine;
using UnityEngine.VFX;
public abstract class ProjectileBehavior : ScriptableObject
{
    public abstract void HandleCollision(Collider target, float damage, Vector3 hitLocation, Vector3 hitNormal, VisualEffectAsset hitFXAsset);
}

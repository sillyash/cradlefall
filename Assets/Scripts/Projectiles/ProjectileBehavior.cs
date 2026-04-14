using UnityEngine;
using UnityEngine.VFX;
public abstract class ProjectileBehavior : ScriptableObject
{
    public abstract void HandleCollision(Collider target, float damagel);
}

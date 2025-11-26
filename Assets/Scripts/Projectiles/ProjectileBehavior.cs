using UnityEngine;

public abstract class ProjectileBehavior : ScriptableObject
{
    public abstract void HandleCollision(Collider target, float damage);
}

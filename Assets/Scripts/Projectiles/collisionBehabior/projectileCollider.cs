using UnityEngine;

public class ProjectileCollider : ProjectileBase
{
        void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<ProjectileBase>(out ProjectileBase var))
        {
            behavior.HandleCollision(other, stats.damage);
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class ProjectileCollider : ProjectileBase
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out ProjectileBase var))
        {
            if (!gameObject.CompareTag("VisualOnly")) {
                behavior.HandleCollision(other, stats.damage);
            }

            Destroy(gameObject);
        }
    }
}

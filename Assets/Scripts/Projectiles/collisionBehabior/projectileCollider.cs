using UnityEngine;
using UnityEngine.VFX;

public class ProjectileCollider : ProjectileBase
{ 
    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out ProjectileBase var))
        {

            if (!gameObject.CompareTag("VisualOnly")) {
                Vector3 hit = other.ClosestPoint(transform.position);

                Vector3 normal = -transform.forward.normalized;

                behavior.HandleCollision(other, stats.damage);
            }

            Destroy(gameObject);
        }
    }
}

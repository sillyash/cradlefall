using UnityEngine;
using UnityEngine.VFX;

public class ProjectileCollider : ProjectileBase
{ 
    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out ProjectileBase var))
        {
            VisualEffect hitFX = spawnFX(other.gameObject);

            if (!gameObject.CompareTag("VisualOnly")) {
                behavior.HandleCollision(other, stats.damage, hitFX);
            }

            Destroy(gameObject);
        }
    }
}

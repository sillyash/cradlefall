using System;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(menuName = "Projectiles/Behaviours/Player")]
public class PlayerProjectile : ProjectileBehavior
{

    public override void HandleCollision(Collider target, float damage, VisualEffect hitFX)
    {
        BaseEnemy enemy = target.GetComponent<BaseEnemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);

            if (hitFX != null)
            {
                hitFX.Play();
            }
        }
    }
}

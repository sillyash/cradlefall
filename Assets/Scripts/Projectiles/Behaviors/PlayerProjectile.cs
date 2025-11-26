using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Projectiles/Behaviours/Player")]
public class PlayerProjectile : ProjectileBehavior
{

    public override void HandleCollision(Collider target, float damage)
    {
        baseEnemies enemy = target.GetComponent<baseEnemies>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}

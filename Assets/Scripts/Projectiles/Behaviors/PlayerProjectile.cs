using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Projectiles/Behaviours/Player")]
public class PlayerProjectile : ProjectileBehavior
{

    public override void HandleCollision(Collider target, float damage)
    {
        BaseEnemy enemy = target.GetComponent<BaseEnemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}

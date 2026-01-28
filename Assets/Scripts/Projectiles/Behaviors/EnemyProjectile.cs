using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Projectiles/Behaviours/Enemy")]
public class EnemyProjectile : ProjectileBehavior
{
    
    public override void HandleCollision(Collider target, float damage)
    {
        basePlayer player = target.GetComponent<basePlayer>();

        if (player != null)
        {
            player.TakeDamage(damage);
        }
        else
        {
            BaseModule module = target.GetComponent<BaseModule>();

            if (module != null)
            {
                module.Break();
            }
        }
    }
}

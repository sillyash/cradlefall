using System;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(menuName = "Projectiles/Behaviours/Enemy")]
public class EnemyProjectile : ProjectileBehavior
{
    // TODO : Rework 
    
    public override void HandleCollision(Collider target, float damage, VisualEffect hitFX)
    {
        basePlayer player = target.GetComponent<basePlayer>();

        if (player != null)
        {
            player.TakeDamage(damage);
            hitFX.Play();
        }
        else
        {
            BaseModule module = target.GetComponent<BaseModule>();

            if (module != null)
            {
                // module.Break(); TODO: to handle 
            }
        }
    }
}

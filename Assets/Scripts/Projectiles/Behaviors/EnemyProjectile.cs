using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Projectiles/Behaviours/Enemy")]
public class EnemyProjectile : ProjectileBehavior
{
    // TODO : Rework 
    
    public override void HandleCollision(Collider target, float damage)
    {
        PlayerController player = target.GetComponent<PlayerController>();

        if (player != null)
        {
            throw new NotImplementedException("Damage not implemented for player");
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

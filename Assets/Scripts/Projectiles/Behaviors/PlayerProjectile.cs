using System;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

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

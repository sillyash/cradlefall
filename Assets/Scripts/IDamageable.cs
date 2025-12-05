using UnityEngine;

public interface IDamageable
{
    float health { get; set; }
    float maxHealth { get; set; }
    void TakeDamage(float damage);
}

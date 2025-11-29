using UnityEngine;

public interface IDamageable
{
    float health { get; set; }

    bool isDead { get; set; }

    void TakeDamage(float dammage);
    void Die();
}

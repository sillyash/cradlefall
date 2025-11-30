using UnityEngine;

public interface IDamageable
{
    bool isDead { get; set; }

    void TakeDamage(float dammage);
    void Die();
}

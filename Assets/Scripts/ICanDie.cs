using UnityEngine;

public interface ICanDie
{
    bool isDead { get; set; }

    void Die();
}

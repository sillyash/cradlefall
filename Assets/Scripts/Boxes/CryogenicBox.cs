using UnityEngine;

public class CryogenicBox : MonoBehaviour, IDamageable
{
    public float health { get; set; }
    public float maxHealth { get; set; } = 500;

    private bool broken = false;

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (broken) return;

        health -= amount;

        if (health <= 0)
        {
            Break();
        }
    }

    public void Break()
    {
        if (broken) return;
        broken = true;
        GameManager.instance.GameOver();
    }

    public void Heal(float ammount)
    {
        if (health < (0.75*maxHealth))
        {
            health += ammount;
        }
    }

    public bool IsBroken()
    {
        return broken;
    }

    public float Gethealth()
    {
        return health;
    }
}
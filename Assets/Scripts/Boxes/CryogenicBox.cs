using UnityEngine;

public class CryogenicBox : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxPV = 500;
    [SerializeField] private float currentPV;
    private bool broken = false;

    void Start()
    {
        currentPV = maxPV;
    }

    public void TakeDamage(float amount)
    {
        if (broken) return;

        currentPV -= amount;

        if (currentPV <= 0)
        {
            Break();
        }
    }

    public void Break()
    {
        if (broken) return;
        broken = true;
    }

    public void Heal(float ammount)
    {
        if (currentPV < (0.75*maxPV))
        {
            currentPV += ammount;
        }
    }

    public bool IsBroken()
    {
        return broken;
    }

    public float GetCurrentPV()
    {
        return currentPV;
    }
}
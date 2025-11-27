using UnityEngine;

public class CryogenicBox : MonoBehaviour
{
    [SerializeField] private int maxPV = 100;
    private int currentPV;
    private bool broken = false;

    void Start()
    {
        currentPV = maxPV;
    }

    public void TakeDamage(int amount)
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

    public void Repair()
    {
        if (!broken) return;
        broken = false;
        currentPV = maxPV;
    }

    public bool IsBroken()
    {
        return broken;
    }

    public int GetCurrentPV()
    {
        return currentPV;
    }
}
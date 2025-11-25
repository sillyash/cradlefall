using UnityEngine;

public class baseWeapon : MonoBehaviour
{
    //[SerializeField] private WeaponBehavior behavior;
    [SerializeField] private WeaponStats stats;

    public int ammoCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ammoCount = stats.magSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

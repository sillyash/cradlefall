using UnityEngine;

[CreateAssetMenu(fileName = "statsArmes", menuName = "Weapon/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    public int magSize;
    public float reloadTime;
    public float shootingCooldown;
}

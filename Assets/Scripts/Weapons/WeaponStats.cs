using UnityEngine;

[CreateAssetMenu(fileName = "statsArmes", menuName = "Weapon/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    public float magSize;
    public float reloadTime;
    public float shootingCooldown;
}

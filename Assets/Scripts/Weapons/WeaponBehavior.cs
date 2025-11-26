using UnityEngine;

[CreateAssetMenu(fileName = "WeaponBehavior", menuName = "Weapon/WeaponBehavior")]
public abstract class WeaponBehavior : ScriptableObject
{
    public abstract void OnFire(ProjectileBase projectile, Vector3 position);
}

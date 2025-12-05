using UnityEngine;

[CreateAssetMenu(fileName = "WeaponBehavior", menuName = "Weapon/WeaponBehavior")]
public abstract class WeaponBehavior : ScriptableObject
{
    public abstract void OnFire(ProjectileBase projectile, Transform transform, float spread);

    public void spreadValue(Quaternion rotation, float val)
    {
        rotation.x += Random.Range(-30, 30);
        rotation.y += Random.Range(-30, 30);
    }
}

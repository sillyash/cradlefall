using UnityEngine;

[CreateAssetMenu(fileName = "WeaponBehavior", menuName = "Weapon/WeaponBehavior")]
public abstract class WeaponBehavior : ScriptableObject
{
    public abstract void OnFire(ProjectileBase projectile, Transform transform, float spread);
    public abstract Quaternion CalcSpread(Transform projectile, float spread);

    public Vector3 spreadValue(Vector3 angles, float val)
    {
        val /= 2;

        angles.x += Random.Range(-val, val);
        angles.y += Random.Range(-val, val);

        return angles;
    }
}

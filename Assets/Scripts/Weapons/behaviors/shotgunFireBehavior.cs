using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Behavior/shotgun")]
public class ShotGunFireBehavior : WeaponBehavior
{
    [SerializeField] private int projectile_per_shot;
    public override void OnFire(ProjectileBase projectile, Transform transform, float spread)
    {

        for(int i=0; i < projectile_per_shot; i++) { 
            // Convert from euler angles to quaternion and from local to global space
            Quaternion spreadedRotation = CalcSpread(transform, spread);

            Instantiate(projectile, transform.position, spreadedRotation);
        }
    }

    public override Quaternion CalcSpread(Transform thing, float spread)
    {
        Vector3 spreadedAngles = spreadValue(thing.transform.localEulerAngles, spread);
        return Quaternion.Euler(spreadedAngles) * thing.transform.rotation;
    }

}

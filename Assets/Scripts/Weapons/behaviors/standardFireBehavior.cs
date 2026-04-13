using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Behavior/standard")]
public class TestBehavior : WeaponBehavior
{
    public override void OnFire(ProjectileBase projectile, Transform transform, float spread)
    {

        // Convert from euler angles to quaternion and from local to global space
        Quaternion spreadedRotation = CalcSpread(transform, spread);

        Instantiate(projectile, transform.position, spreadedRotation);

    }

    public override Quaternion CalcSpread(Transform thing, float spread)
    {
        Vector3 spreadedAngles = spreadValue(thing.transform.localEulerAngles, spread);
        return Quaternion.Euler(spreadedAngles) * thing.transform.rotation;
    }

}

using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Behaviours/test")]
public class TestBehavior : WeaponBehavior
{
    public override void OnFire(ProjectileBase projectile, Transform transform, float spread)
    {
        Vector3 spreadedAngles = spreadValue(transform.localEulerAngles, spread);

        // Convert from euler angles to quaternion and from local to global space
        Quaternion spreadedRotation = Quaternion.Euler(spreadedAngles) * transform.rotation;

        Instantiate(projectile, transform.position, spreadedRotation);

    }

}

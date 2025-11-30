using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Behaviours/test")]
public class TestBehavior : WeaponBehavior
{
    public override void OnFire(ProjectileBase projectile, Transform transform)
    {

        ProjectileBase proj = Instantiate(projectile, transform.position, transform.rotation);
    }

}

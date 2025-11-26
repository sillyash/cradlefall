using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Behaviours/test")]
public class TestBehavior : WeaponBehavior
{
    public override void OnFire(ProjectileBase projectile, Vector3 position)
    {
        ProjectileBase proj = Instantiate(projectile, position, Quaternion.identity);

        proj.setDirection(new Vector3(0, 0, -1));
    }

}

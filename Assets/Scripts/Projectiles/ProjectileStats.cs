using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileStats", menuName = "Projectiles/stats")]
public class ProjectileStats : ScriptableObject
{
    public float speed;
    public float damage;
    public float lifespan;
    public AnimationCurve launchCurve; //Currently does nothing
}

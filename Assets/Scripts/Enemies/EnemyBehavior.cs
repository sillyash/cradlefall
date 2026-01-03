using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBehavior", menuName = "Enemy/Behavior")]
public abstract class EnemyBehavior : ScriptableObject
{
    public abstract void Attack();
}

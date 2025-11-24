using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesBehavior", menuName = "Enemies/behavior")]
public abstract class EnemiesBehavior : ScriptableObject
{
    public abstract void attack();
}

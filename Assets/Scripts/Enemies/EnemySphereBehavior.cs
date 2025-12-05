using Unity.VisualScripting;
using UnityEngine;

public class EnemySphereBehavior : MonoBehaviour
{
    public GameObject me;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable) && !(damageable is baseEnemies))
        {
            baseEnemies myBase = me.GetComponent<baseEnemies>();
            damageable.TakeDamage(myBase.stats.damage);
        }
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class EnemySphereBehavior : MonoBehaviour
{
    public GameObject me;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            baseEnemies myBase = me.GetComponent<baseEnemies>();
            Debug.Log(myBase.stats.damage);
            damageable.TakeDamage(myBase.stats.damage);
        }
    }
}

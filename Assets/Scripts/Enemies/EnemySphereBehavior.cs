using Unity.VisualScripting;
using UnityEngine;

public class EnemySphereBehavior : MonoBehaviour
{
    public GameObject me;
    public bool hasTouched = false;
    private void OnTriggerEnter(Collider other)
    {
        if (hasTouched) return;
        if (other.TryGetComponent<IDamageable>(out var damageable) && !(damageable is BaseEnemy))
        {
            BaseEnemy myBase = me.GetComponent<BaseEnemy>();
            hasTouched = true;
            damageable.TakeDamage(myBase.stats.damage);
        }
    }
}

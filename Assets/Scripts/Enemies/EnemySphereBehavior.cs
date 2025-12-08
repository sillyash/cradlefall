using Unity.VisualScripting;
using UnityEngine;

public class EnemySphereBehavior : MonoBehaviour
{
    public GameObject me;
    public bool hasTouched = false;
    private void OnTriggerEnter(Collider other)
    {
        if (hasTouched) return;
        if (other.TryGetComponent<IDamageable>(out var damageable) && !(damageable is baseEnemies))
        {
            baseEnemies myBase = me.GetComponent<baseEnemies>();
            hasTouched = true;
            damageable.TakeDamage(myBase.stats.damage);
        }
    }
}

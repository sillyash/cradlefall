using UnityEngine;

public class ProjectileRaycast : ProjectileBase
{

    [SerializeField] private float radius;

    private Vector3 lastPosition;

    new void Start()
    {
        base.Start();
        lastPosition = transform.position;
        radius *= transform.localScale.y;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, radius * transform.localScale.y);
    }
    new void Update()
    {
        base.Update();
        float distance = (transform.position - lastPosition).magnitude;

        if (Physics.SphereCast(transform.position, radius, -transform.forward, out RaycastHit hit, distance))
        {
            if (!hit.collider.TryGetComponent(out ProjectileBase hitObject))
            {
                if (!gameObject.CompareTag("VisualOnly"))
                {
                    behavior.HandleCollision(hit.collider, stats.damage);
                }
                Destroy(gameObject);
            }
        }
    }
}

using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class ProjectileBase : MonoBehaviour
{
    [SerializeField] private ProjectileBehavior behavior;
    [SerializeField] private ProjectileStats stats;

    // Will not stay serialized, but it's useful when testing
    [SerializeField] private Vector3 direction;
    private Rigidbody rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(behavior == null)
        {
            Debug.Log("No behavior assigned");
        }

        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = direction.normalized * stats.speed;
        //rb.AddForce(direction.normalized * speed, ForceMode.Impulse);
    }

    void OnTriggerEnter(Collider other)
    {
        behavior.HandleCollision(other, stats.damage);
        Destroy(gameObject);
    }


    // Fixed update is basically update with delta time
    void FixedUpdate()
    {
        UpdateRotation();
    }
    void UpdateRotation()
    {
        // Ignore rotation if velocity is too small
        if (rb.linearVelocity.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(rb.linearVelocity);
            rb.MoveRotation(targetRot);
        }
    }

    public void setDirection(Vector3 dir)
    {
        direction = dir;
    }
}

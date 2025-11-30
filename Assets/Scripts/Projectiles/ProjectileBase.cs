using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI.Table;


public class ProjectileBase : MonoBehaviour
{
    public static float CURVE_AMP = 100;
    public static float CURVE_LENGTH_DIV = 2;
    [SerializeField] private ProjectileBehavior behavior;
    [SerializeField] private ProjectileStats stats;

    private Rigidbody rb;

    // To calculate curve tangent
    private Vector3 initPos;
    private float prevCurvVal = 0;
    private float curveVal;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (behavior == null)
        {
            Debug.Log("No behavior assigned");
        }

        if (stats == null)
        {
            Debug.Log("No stats assigned");
        }

        rb = GetComponent<Rigidbody>();

        rb.linearVelocity = transform.forward * stats.speed;
        initPos = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<ProjectileBase>(out ProjectileBase var))
        {
            behavior.HandleCollision(other, stats.damage);
            Destroy(gameObject);
        }
    }


    // Fixed update is basically update with delta time
    void FixedUpdate()
    {
        
        Vector3 vel = rb.linearVelocity;

        float point = Vector3.Distance(transform.position, initPos) / CURVE_LENGTH_DIV;

        //vel.y = getCurveOrientation(point);

        prevCurvVal = curveVal;

        //rb.linearVelocity = vel;
        UpdateRotation();
    }

    // Return the tangent on the curve in stat at given point
    float getCurveOrientation(float point)
    {
        curveVal = stats.launchCurve.Evaluate(point);

        return (curveVal - prevCurvVal) * CURVE_AMP;
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
}

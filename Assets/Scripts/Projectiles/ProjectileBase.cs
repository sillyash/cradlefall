using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    public static float CURVE_AMP = 100;
    public static float CURVE_LENGTH_DIV = 2;
    public static float LIFESPAN = 2; // in seconds

    [SerializeField] protected ProjectileBehavior behavior;
    [SerializeField] protected ProjectileStats stats;

    private Rigidbody rb;

    // To calculate curve tangent
    private Vector3 initPos;
    private float startTime;
    private float prevCurvVal = 0;
    private float curveVal;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
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
        startTime = Time.time;
    }

    protected void Update()
    {
        if (Time.time - startTime > LIFESPAN)
        {
            Destroy(gameObject);
        }
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

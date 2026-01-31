using UnityEngine;

public class DeathCamera : MonoBehaviour
{
    public float yaw = 6;
    public float tilt = 15;
    private void Start()
    {
        KeyMappingSettings.ApplyPresetZQSD();
    }
    void Update()
    {
        HandleLook();
    }

    void HandleLook()
    {
        float axisX = CustomInputRouter.GetAxisHorizontal();
        float axisY = CustomInputRouter.GetAxisVertical();

        yaw += axisX * 30 * Time.deltaTime;
        tilt += axisY * 30 * Time.deltaTime;

        yaw = Mathf.Clamp(yaw, -60f, 60f);
        tilt = Mathf.Clamp(tilt, -40f, 40f);

        Debug.Log("yaw " + yaw + "tilt " + tilt);

        transform.rotation = Quaternion.Euler(-tilt, yaw, 0f);
    }
}

using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private bool lockXRotation = true;

    void Update()
    {
        if (Camera.main == null)
            return;

        Vector3 toCamera = Camera.main.transform.position - transform.position;
        if (toCamera.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(toCamera.normalized);
        if (lockXRotation)
        {
            Vector3 euler = targetRotation.eulerAngles;
            targetRotation = Quaternion.Euler(0f, euler.y, 0f);
        }

        transform.rotation = targetRotation;
    }
}

using UnityEngine;

[DefaultExecutionOrder(100)] // CameraFollow(LateUpdate)보다 '늦게' 실행되도록
public class DashCamera : MonoBehaviour
{
    [SerializeField]
    private DashAbility dash;

    [SerializeField]
    private Camera targetCamera;

    [SerializeField]
    private float zoomAmount = 0.4f;

    [SerializeField]
    private float zoomDuration = 0.12f;

    [SerializeField]
    private float shakeAmplitude = 0.15f;

    [SerializeField]
    private float shakeDuration = 0.10f;

    private bool wasDashing = false;
    private float zoomTimer = 0.0f;
    private float shakeTimer = 0.0f;

    private float dashBaseOrthoSize = 5.0f;

    private void Awake()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        if (targetCamera != null)
        {
            dashBaseOrthoSize = targetCamera.orthographicSize;
        }
    }

    private void LateUpdate()
    {
        Vector3 basePosition = transform.position;

        bool isDashing = false;

        if (dash != null)
        {
            isDashing = dash.GetIsDashing();
        }

        if (wasDashing == false && isDashing == true)
        {
            zoomTimer = zoomDuration;
            shakeTimer = shakeDuration;

            if (targetCamera != null)
            {
                dashBaseOrthoSize = targetCamera.orthographicSize;
            }
        }

        if (targetCamera != null && zoomTimer > 0.0f)
        {
            zoomTimer = zoomTimer - Time.deltaTime;

            float t = 1.0f - (zoomTimer / zoomDuration);

            if (t < 0.0f)
            {
                t = 0.0f;
            }

            if (t > 1.0f)
            {
                t = 1.0f;
            }

            float size = Mathf.Lerp(dashBaseOrthoSize - zoomAmount, dashBaseOrthoSize, t);
            targetCamera.orthographicSize = size;
        }
        if (shakeTimer > 0.0f)
        {
            shakeTimer = shakeTimer - Time.deltaTime;

            float strength = shakeTimer / shakeDuration;

            float dx = (Random.value * 2.0f - 1.0f) * shakeAmplitude * strength;
            float dy = (Random.value * 2.0f - 1.0f) * shakeAmplitude * strength;

            Vector3 offset = new Vector3(dx, dy, 0.0f);

            transform.position = basePosition + offset;
        }

        wasDashing = isDashing;
    }
}

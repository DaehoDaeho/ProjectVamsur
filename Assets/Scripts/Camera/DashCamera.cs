using System.Threading;
using UnityEngine;

public class DashCamera : MonoBehaviour
{
    [SerializeField]
    private DashAbility dash;

    [SerializeField]
    private Camera targetCamera;

    [SerializeField]
    private float zoomAmount = 0.4f;

    [SerializeField]
    private float zoomDuration = 0.1f;

    [SerializeField]
    private float shakeAmplifier = 0.15f;

    [SerializeField]
    private float shakeDuration = 0.1f;

    [SerializeField]
    private CameraFollow cameraFollow;

    private bool wasDashing = false;
    private float zoomTimer = 0.0f;
    private float shakeTimer = 0.0f;
    private float originalSize = 5.0f;
    private Vector3 originalPos;

    private void Awake()
    {
        originalSize = targetCamera.orthographicSize;
        originalPos = transform.localPosition;
        //originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        bool isDashing = dash.GetIsDashing();

        if(wasDashing == false && isDashing == true)
        {
            zoomTimer = zoomDuration;
            shakeTimer = shakeDuration;
            cameraFollow.enabled = false;
        }

        if(zoomTimer > 0.0f && targetCamera != null)
        {
            zoomTimer -= Time.deltaTime;
            float t = 1.0f - (zoomTimer / zoomDuration);

            if(t < 0.0f)
            {
                t = 0.0f;
            }

            if(t > 1.0f)
            {
                t = 1.0f;
            }

            float size = Mathf.Lerp(originalSize - zoomAmount, originalSize, t);
            targetCamera.orthographicSize = size;
        }
        else
        {
            if(wasDashing == true)
            {
                targetCamera.orthographicSize = originalSize;
                transform.position = originalPos;
                cameraFollow.enabled = true;
            }
        }

        if(shakeTimer > 0.0f)
        {
            //shakeTimer -= Time.deltaTime;

            //float strength = shakeTimer / shakeDuration;
            //float dx = (Random.value * 2.0f - 1.0f) * shakeAmplifier * strength;
            //float dy = (Random.value * 2.0f - 1.0f) * shakeAmplifier * strength;

            //transform.localPosition = originalPos + new Vector3(dx, dy, 0.0f);
            //transform.position = originalPos + new Vector3(dx, dy, 0.0f);
        }
        else
        {
            if(wasDashing == true)
            {
                //transform.localPosition = originalPos;
                //transform.position = originalPos;
            }

            //cameraFollow.enabled = true;
        }

        wasDashing = isDashing;
    }
}

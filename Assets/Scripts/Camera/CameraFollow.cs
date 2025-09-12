using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float followLerp = 8.0f;

    [SerializeField]
    private Vector3 offset = Vector3.zero;

    [SerializeField]
    private MapBounds mapBounds;

    [SerializeField]
    private Camera cam;

    private void LateUpdate()
    {
        if(target != null)
        {
            float minX;
            float maxX;
            float minY;
            float maxY;
            mapBounds.GetWorldBounds(out minX, out maxX, out minY, out maxY);

            float halfHeight = cam.orthographicSize;
            float halfWidth = halfHeight * cam.aspect;

            float camMinX = minX + halfWidth;
            float camMaxX = maxX - halfWidth;
            float camMinY = minY + halfHeight;
            float camMaxY = maxY - halfHeight;

            float camX = target.position.x;
            float camY = target.position.y;

            if(camMinX > camMaxX)
            {
                camX = (minX + maxX) * 0.5f;
            }
            else
            {
                camX = Mathf.Clamp(camX, camMinX, camMaxX);
            }

            if (camMinY > camMaxY)
            {
                camY = (minY + maxY) * 0.5f;
            }
            else
            {
                camY = Mathf.Clamp(camY, camMinY, camMaxY);
            }

            //Vector3 goal = new Vector3(target.position.x, target.position.y, target.position.z);
            Vector3 goal = new Vector3(camX, camY, target.position.z);
            goal = goal + offset;

            float t = followLerp * Time.deltaTime;
            if (t > 1.0f)
            {
                t = 1.0f;
            }

            // 焊埃 - 何靛矾款 贸府.
            transform.position = Vector3.Lerp(transform.position, goal, t);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}

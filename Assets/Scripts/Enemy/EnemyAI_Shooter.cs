using UnityEngine;

public class EnemyAI_Shooter : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3.0f;

    [SerializeField]
    private float desiredRange = 5.0f;

    [SerializeField]
    private float deadZone = 0.5f;

    [SerializeField]
    private Projectile2D projectilePrefab;

    [SerializeField]
    private float fireIntervalSeconds = 1.5f;

    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private Rigidbody2D body;

    [SerializeField]
    private TargetLocator locator;

    private float fireTimer = 0.0f;

    private void Awake()
    {
        fireTimer = fireIntervalSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        if(locator == null)
        {
            return;
        }

        Transform player = locator.playerTransform;

        if(player == null)
        {
            return;
        }

        Vector2 toPlayer = player.position - transform.position;
        float distance = toPlayer.magnitude;

        Vector2 v = Vector2.zero;
        if(distance > desiredRange + deadZone)
        {
            v = toPlayer.normalized * moveSpeed;
        }
        else if(distance < desiredRange - deadZone)
        {
            v = (-toPlayer).normalized * moveSpeed;
        }
        else
        {
            v = Vector2.zero;
        }

        body.linearVelocity = v;

        if(fireTimer > 0.0f)
        {
            fireTimer -= Time.deltaTime;
        }

        if(fireTimer <= 0.0f)
        {
            FireAt(player.position);
            fireTimer = fireIntervalSeconds;
        }
    }

    void FireAt(Vector3 targetPos)
    {
        if(projectilePrefab == null)
        {
            return;
        }

        Vector2 origin = transform.position;

        if(firePoint != null)
        {
            origin = firePoint.position;
        }

        Vector2 dir = (targetPos - (Vector3)origin).normalized;

        Projectile2D p = Instantiate(projectilePrefab, origin, Quaternion.identity);
        if(p != null)
        {
            p.Fire(dir);
        }
    }
}

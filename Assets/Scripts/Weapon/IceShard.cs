using UnityEngine;

public class IceShard : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float damage;
    private float lifetime = 3f;
    private Rigidbody2D rb;
    private bool hasHit;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        if (!hasHit)
        {
            rb.linearVelocity = direction * speed;
        }
    }

    public void Initialize(Vector2 dir, float spd, float dmg)
    {
        direction = dir.normalized;
        speed = spd;
        damage = dmg;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (hasHit) return;

        if (col.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = col.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            hasHit = true;
            //Destroy(gameObject);
        }
        else if (col.CompareTag("Wall") || col.CompareTag("Obstacle"))
        {
            hasHit = true;
            Destroy(gameObject);
        }
    }
}
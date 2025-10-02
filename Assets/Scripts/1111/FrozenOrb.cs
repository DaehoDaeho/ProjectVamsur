using UnityEngine;

// Frozen Orb ����ü - õõ�� ���ư��ٰ� �����ϴ� ��ü
public class FrozenOrb : MonoBehaviour
{
    [Header("Orb Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private float explosionDelay = 1f;

    [Header("Explosion Settings")]
    [SerializeField] private int shardCount = 16;
    [SerializeField] private float shardSpeed = 8f;
    [SerializeField] private GameObject iceShardPrefab;

    [Header("Damage")]
    [SerializeField] private float orbDamage = 10f;
    [SerializeField] private float shardDamage = 5f;

    [Header("Visual")]
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private TrailRenderer trail;

    private Vector2 direction;
    private float timer;
    private bool hasExploded;
    private Rigidbody2D rb;

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

    void Update()
    {
        timer += Time.deltaTime;

        // ���� �ð��� �Ǹ� ����
        if (!hasExploded && timer >= explosionDelay)
        {
            //Explode();
        }
    }

    void FixedUpdate()
    {
        if (!hasExploded)
        {
            rb.linearVelocity = direction * moveSpeed;
        }
    }

    public void Initialize(Vector2 shootDirection)
    {
        direction = shootDirection.normalized;

        // ���⿡ ���� ȸ��
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Explode()
    {
        hasExploded = true;
        rb.linearVelocity = Vector2.zero;

        // ���� ���� �߻�
        SpawnIceShards();

        // ���� ����Ʈ
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Trail ����
        if (trail != null)
        {
            trail.transform.SetParent(null);
            Destroy(trail.gameObject, trail.time);
        }

        Destroy(gameObject);
    }

    void SpawnIceShards()
    {
        if (iceShardPrefab == null) return;

        float angleStep = 360f / shardCount;

        for (int i = 0; i < shardCount; i++)
        {
            float angle = i * angleStep;
            Vector2 shardDir = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );

            GameObject shard = Instantiate(iceShardPrefab, transform.position, Quaternion.identity);
            IceShard shardScript = shard.GetComponent<IceShard>();

            if (shardScript != null)
            {
                shardScript.Initialize(shardDir, shardSpeed, shardDamage);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // ���� �浹 �� ���ظ� ������ ����
        if (col.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = col.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(orbDamage);
                if (!hasExploded)
                {
                    Explode();
                }
            }
        }

        // ���� �浹 �� ��� ����
        if (col.CompareTag("Wall") || col.CompareTag("Obstacle"))
        {
            if (!hasExploded)
            {
                Explode();
            }
        }
    }
}
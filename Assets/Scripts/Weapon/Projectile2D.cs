using UnityEngine;

public class Projectile2D : MonoBehaviour
{
    [SerializeField]
    private float speed = 8.0f;

    [SerializeField]
    private int damage = 8;

    [SerializeField]
    private float lifeTime = 3.0f;

    [SerializeField]
    private Rigidbody2D body;

    private float lifeTimer = 0.0f;
    private Vector2 moveDir = Vector2.right;

    public void Fire(Vector2 dir)
    {
        moveDir = dir.normalized;

        if(body != null)
        {
            body.linearVelocity = moveDir * speed;
        }
    }

    private void Awake()
    {
        lifeTimer = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(lifeTimer > 0.0f)
        {
            lifeTimer -= Time.deltaTime;

            if(lifeTimer <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") == true)
        {
            PlayerStats ps = collision.GetComponent<PlayerStats>();
            if(ps != null)
            {
                ps.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}

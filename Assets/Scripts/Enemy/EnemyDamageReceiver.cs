using UnityEngine;

public class EnemyDamageReceiver : MonoBehaviour
{
    [SerializeField]
    private EnemyHealth enemyHealth;

    [SerializeField]
    private Rigidbody2D body;

    [SerializeField]
    private float knockStartSpeed = 6.0f;

    [SerializeField]
    private float knockDuration = 0.2f;

    [SerializeField]
    private float knockDecaySeconds = 25.0f;

    [SerializeField]
    private float knockCooldownSeconds = 0.05f;

    private Vector2 knockVel = Vector2.zero;
    private float knockTimer = 0.0f;
    private float knockCooldownTimer = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(knockTimer > 0.0f)
        {
            if(body != null)
            {
                body.linearVelocity = knockVel;
            }

            float reduce = knockDecaySeconds * Time.deltaTime;

            if(knockVel.magnitude > reduce)
            {
                knockVel = knockVel.normalized * (knockVel.magnitude - reduce);
            }
            else
            {
                knockVel = Vector2.zero;
            }

            knockTimer -= Time.deltaTime;
            if(knockTimer <= 0.0f)
            {
                if(body != null)
                {
                    body.linearVelocity = Vector2.zero;
                }

                knockVel = Vector2.zero;
            }
        }

        if(knockCooldownTimer > 0.0f)
        {
            knockCooldownTimer -= Time.deltaTime;
        }
    }

    public void ApplyHit(int damage, Vector2 knockbackDir)
    {
        if(enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);

            if(enemyHealth.IsDead() == true)
            {
                return;
            }
        }

        if(knockCooldownTimer > 0.0f)
        {
            return;
        }

        Vector2 dir = knockbackDir.normalized;
        knockVel = dir * knockStartSpeed;
        knockTimer = knockDuration;
        knockCooldownTimer = knockCooldownSeconds;
    }
}

using UnityEngine;

public class EnemyAI_Rusher : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3.5f;

    [SerializeField]
    private float touchDamage = 10;

    [SerializeField]
    private float touchCooldownSeconds = 0.5f;

    [SerializeField]
    private Rigidbody2D body;

    [SerializeField]
    private TargetLocator locator;

    private float touchTimer = 0.0f;
        
    // Update is called once per frame
    void Update()
    {
        if(touchTimer > 0.0f)
        {
            touchTimer -= Time.deltaTime;
        }

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
        Vector2 dir = toPlayer.normalized;
        Vector2 v = dir * moveSpeed;

        body.linearVelocity = v;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(touchTimer > 0.0f)
        {
            return;
        }

        PlayerStats ps = collision.gameObject.GetComponent<PlayerStats>();
        if (ps != null)
        {
            ps.TakeDamage(touchDamage);
            touchTimer = touchCooldownSeconds;
        }
    }
}

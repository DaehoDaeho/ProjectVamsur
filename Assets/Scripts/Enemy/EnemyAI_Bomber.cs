using UnityEngine;

public class EnemyAI_Bomber : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3.5f;

    [SerializeField]
    private float explodeDistance = 1.4f;

    [SerializeField]
    private float telegraphSeconds = 0.6f;

    [SerializeField]
    private float explosionRadius = 2.0f;

    [SerializeField]
    private int explosionDamage = 20;

    [SerializeField]
    private LayerMask playerLayerMask;

    [SerializeField]
    private SpriteRenderer bodyRenderer;

    [SerializeField]
    private Color telegraphColor = new Color(1.0f, 0.5f, 0.5f, 1.0f);

    [SerializeField]
    private Rigidbody2D body;

    [SerializeField]
    private TargetLocator locator;

    private bool primed = false;
    private float telegraphTimer = 0.0f;
    private Color baseColor;

    private void Awake()
    {
        if(bodyRenderer != null)
        {
            baseColor = bodyRenderer.color;
        }
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

        if(primed == false)
        {
            Vector2 toPlayer = player.position - transform.position;
            float distance = toPlayer.magnitude;

            if(distance > explodeDistance)
            {
                body.linearVelocity = toPlayer.normalized * moveSpeed;
            }
            else
            {
                primed = true;
                telegraphTimer = telegraphSeconds;
                body.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            if(bodyRenderer != null)
            {
                float t = 0.5f + 0.5f * Mathf.Sin(Time.time * Mathf.PI * 16.0f);
                bodyRenderer.color = Color.Lerp(baseColor, telegraphColor, t);
            }

            telegraphTimer -= Time.deltaTime;
            if(telegraphTimer <= 0.0f)
            {
                Explode();
            }
        }
    }

    void Explode()
    {
        if(bodyRenderer != null)
        {
            bodyRenderer.color = baseColor;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, playerLayerMask);

        if(hits != null)
        {
            for(int i=0; i<hits.Length; ++i)
            {
                PlayerStats ps = hits[i].GetComponent<PlayerStats>();
                if(ps != null)
                {
                    ps.TakeDamage(explosionDamage);
                }
            }
        }

        Destroy(gameObject);
    }
}

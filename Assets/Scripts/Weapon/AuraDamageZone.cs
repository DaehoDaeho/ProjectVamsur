using UnityEngine;

public class AuraDamageZone : MonoBehaviour
{
    [SerializeField]
    private float radius = 3.0f;

    [SerializeField]
    private LayerMask enemyLayerMask;

    [SerializeField]
    private float damagePerSecond = 10.0f;

    [SerializeField]
    private float tickIntervalSeconds = 0.25f;

    [SerializeField]
    private SpriteRenderer auraRenderer;

    [SerializeField]
    private float pulseSpeed = 2.0f;

    [SerializeField]
    private float minAlpha = 0.15f;

    [SerializeField]
    private float maxAlpha = 0.35f;

    private float tickTimer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if(tickTimer > 0.0f)
        {
            tickTimer -= Time.deltaTime;
        }

        if (tickTimer <= 0.0f)
        {
            DoTick();
            tickTimer = tickIntervalSeconds;
        }

        if(auraRenderer != null)
        {
            Color c = auraRenderer.color;

            float t = 0.5f + 0.5f * Mathf.Sin(Time.time * Mathf.PI * 2.0f * pulseSpeed);
            c.a = Mathf.Lerp(minAlpha, maxAlpha, t);
            auraRenderer.color = c;
        }
    }

    void DoTick()
    {
        int tickDamage = Mathf.RoundToInt(damagePerSecond * tickIntervalSeconds);

        if(tickDamage <= 0)
        {
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayerMask);

        if(hits == null)
        {
            return;
        }

        for(int i=0; i<hits.Length; ++i)
        {
            Collider2D c = hits[i];
            if (c == null)
            {
                continue;
            }

            EnemyHealth h = c.GetComponent<EnemyHealth>();
            if(h != null)
            {
                h.TakeDamage(tickDamage);

                EnemyHitFlash flash = c.GetComponent<EnemyHitFlash>();

                if(flash != null)
                {
                    flash.FlashOnce();
                }
            }
        }
    }

    void ApplyVisualScale()
    {
        if(auraRenderer == null)
        {
            return;
        }

        float spriteWidth = 1.0f;

        if(auraRenderer != null)
        {
            spriteWidth = auraRenderer.sprite.bounds.size.x;
        }

        float diameter = radius * 2.0f;
        float scale = diameter / spriteWidth;

        Vector3 s = new Vector3(scale, scale, 1.0f);
        auraRenderer.transform.localScale = s;
    }

    private void OnValidate()
    {
        ApplyVisualScale();
    }
}

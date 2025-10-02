using UnityEngine;

/// <summary>
/// ƽ���� �ݰ� �� ������ ���ظ� �ָ�,
/// ���� ��������Ʈ�� �ݰ濡 ���� �ڵ� �������ϰ� ���� �޽��� �ش�.
/// </summary>
public class AuraDamageZone : MonoBehaviour
{
    [Header("Aura Range")]
    [SerializeField]
    private float radius = 1.8f;                       // [����] ����

    [SerializeField]
    private LayerMask enemyLayerMask;

    [Header("DPS")]
    [SerializeField]
    private float damagePerSecond = 12.0f;             // [����] HP/s

    [SerializeField]
    private float tickIntervalSeconds = 0.25f;         // [����] ��

    [Header("Visual")]
    [SerializeField]
    private SpriteRenderer auraRenderer;               // ���� ��������Ʈ(���� ���� ��)

    [SerializeField]
    private float pulseSpeed = 2.0f;                   // [����] Hz. �ʴ� �޽� Ƚ��

    [SerializeField]
    private float minAlpha = 0.15f;

    [SerializeField]
    private float maxAlpha = 0.35f;

    private float tickTimer = 0.0f;

    private void Start()
    {
        ApplyVisualScale();
    }

    private void Update()
    {
        // ===== ƽ ó�� =====
        if (tickTimer > 0.0f)
        {
            tickTimer = tickTimer - Time.deltaTime;
        }

        if (tickTimer <= 0.0f)
        {
            DoTick();
            tickTimer = tickIntervalSeconds;
        }

        // ===== ���� �޽� =====
        if (auraRenderer != null)
        {
            Color c = auraRenderer.color;

            // [����] t = 0~1 ���� ��� ����. [��] ��¦ �� ���� ������ �޽��� ����.
            float t = 0.5f + 0.5f * Mathf.Sin(Time.time * Mathf.PI * 2.0f * pulseSpeed);
            c.a = Mathf.Lerp(minAlpha, maxAlpha, t);
            auraRenderer.color = c;
        }
    }

    private void DoTick()
    {
        int tickDamage = Mathf.RoundToInt(damagePerSecond * tickIntervalSeconds); // [����] DPS �� ��t

        if (tickDamage <= 0)
        {
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayerMask);

        if (hits == null)
        {
            return;
        }

        for (int i = 0; i < hits.Length; i = i + 1)
        {
            Collider2D c = hits[i];

            if (c == null)
            {
                continue;
            }

            EnemyHealth h = c.GetComponent<EnemyHealth>();

            if (h != null)
            {
                h.TakeDamage(tickDamage);

                // ���� ���� ��½ ����(����)
                EnemyHitFlash flash = c.GetComponent<EnemyHitFlash>();

                if (flash != null)
                {
                    flash.FlashOnce();
                }
            }
        }
    }

    private void ApplyVisualScale()
    {
        if (auraRenderer == null)
        {
            return;
        }

        // [����] ��������Ʈ�� ���� ũ��(����)�� �ݰ��� ����.
        // [��] �ݰ� r �� ���� 2r, ��������Ʈ ������ ���� ������ ���.
        float spriteWidth = 1.0f;

        if (auraRenderer.sprite != null)
        {
            spriteWidth = auraRenderer.sprite.bounds.size.x;
        }

        float diameter = radius * 2.0f;
        float scale = diameter / spriteWidth;

        Vector3 s = new Vector3(scale, scale, 1.0f);
        auraRenderer.transform.localScale = s;
    }

    // �ݰ��� �����Ϳ��� �ٲ���� ���� �ٷ� �ݿ��ϰ� �ʹٸ�:
    private void OnValidate()
    {
        ApplyVisualScale();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

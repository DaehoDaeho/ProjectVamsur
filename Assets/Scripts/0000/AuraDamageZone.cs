using UnityEngine;

/// <summary>
/// 틱마다 반경 내 적에게 피해를 주며,
/// 원형 스프라이트를 반경에 맞춰 자동 스케일하고 투명도 펄스를 준다.
/// </summary>
public class AuraDamageZone : MonoBehaviour
{
    [Header("Aura Range")]
    [SerializeField]
    private float radius = 1.8f;                       // [단위] 유닛

    [SerializeField]
    private LayerMask enemyLayerMask;

    [Header("DPS")]
    [SerializeField]
    private float damagePerSecond = 12.0f;             // [단위] HP/s

    [SerializeField]
    private float tickIntervalSeconds = 0.25f;         // [단위] 초

    [Header("Visual")]
    [SerializeField]
    private SpriteRenderer auraRenderer;               // 원형 스프라이트(알파 낮은 원)

    [SerializeField]
    private float pulseSpeed = 2.0f;                   // [단위] Hz. 초당 펄스 횟수

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
        // ===== 틱 처리 =====
        if (tickTimer > 0.0f)
        {
            tickTimer = tickTimer - Time.deltaTime;
        }

        if (tickTimer <= 0.0f)
        {
            DoTick();
            tickTimer = tickIntervalSeconds;
        }

        // ===== 오라 펄스 =====
        if (auraRenderer != null)
        {
            Color c = auraRenderer.color;

            // [무엇] t = 0~1 사인 기반 보간. [왜] 살짝 숨 쉬는 느낌의 펄스를 위해.
            float t = 0.5f + 0.5f * Mathf.Sin(Time.time * Mathf.PI * 2.0f * pulseSpeed);
            c.a = Mathf.Lerp(minAlpha, maxAlpha, t);
            auraRenderer.color = c;
        }
    }

    private void DoTick()
    {
        int tickDamage = Mathf.RoundToInt(damagePerSecond * tickIntervalSeconds); // [무엇] DPS × Δt

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

                // 맞은 적도 번쩍 가능(선택)
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

        // [무엇] 스프라이트의 가로 크기(유닛)로 반경을 맞춤.
        // [왜] 반경 r → 지름 2r, 스프라이트 폭으로 나눠 스케일 계산.
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

    // 반경이 에디터에서 바뀌었을 때도 바로 반영하고 싶다면:
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

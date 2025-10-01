using UnityEngine;

public enum MeleeStrikeMode
{
    Single,
    Area
}

/// <summary>
/// 자동 근접 공격(모드 전환 가능):
/// - Single: 가장 가까운 적 1명 타격
/// - Area  : 반경 내 여러 명 타격(최대 수 제한)
/// </summary>
public class AutoMeleeStrikeFlex : MonoBehaviour
{
    [Header("Mode")]
    [SerializeField]
    private MeleeStrikeMode mode = MeleeStrikeMode.Single;

    [Header("Range")]
    [SerializeField]
    private float strikeRadius = 1.6f;            // [단위] 유닛. 타격 반경

    [SerializeField]
    private LayerMask enemyLayerMask;              // 대상 레이어

    [Header("Timing")]
    [SerializeField]
    private float swingCooldownSeconds = 0.35f;    // [단위] 초. 발동 간 최소 간격

    [Header("Damage")]
    [SerializeField]
    private int damageOnHit = 10;                  // [단위] HP

    [Header("Area Mode Safety")]
    [SerializeField]
    private int maxTargetsPerStrike = 12;          // [무엇] Area 모드에서 한 번에 처리할 최대 수

    private float cooldownTimer = 0.0f;

    private void Update()
    {
        if (cooldownTimer > 0.0f)
        {
            cooldownTimer = cooldownTimer - Time.deltaTime;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, strikeRadius, enemyLayerMask);

        if (hits == null)
        {
            return;
        }

        if (hits.Length == 0)
        {
            return;
        }

        if (cooldownTimer <= 0.0f)
        {
            if (mode == MeleeStrikeMode.Single)
            {
                StrikeSingle(hits);
            }
            else
            {
                StrikeArea(hits);
            }

            cooldownTimer = swingCooldownSeconds;
        }
    }

    private void StrikeSingle(Collider2D[] hits)
    {
        Transform best = null;
        float bestDist = float.MaxValue;
        Vector3 playerPos = transform.position;

        for (int i = 0; i < hits.Length; i = i + 1)
        {
            Collider2D c = hits[i];

            if (c == null)
            {
                continue;
            }

            float d = (c.transform.position - playerPos).magnitude; // [단위] 유닛

            if (d < bestDist)
            {
                bestDist = d;
                best = c.transform;
            }
        }

        if (best == null)
        {
            return;
        }

        EnemyDamageReceiver r = best.GetComponent<EnemyDamageReceiver>();

        if (r == null)
        {
            return;
        }

        Vector2 dir = (best.position - playerPos).normalized; // [무엇] 외향 방향

        r.ApplyHit(damageOnHit, dir);
    }

    private void StrikeArea(Collider2D[] hits)
    {
        Vector3 playerPos = transform.position;
        int applied = 0;

        for (int i = 0; i < hits.Length; i = i + 1)
        {
            if (applied >= maxTargetsPerStrike)
            {
                break;
            }

            Collider2D c = hits[i];

            if (c == null)
            {
                continue;
            }

            EnemyDamageReceiver r = c.GetComponent<EnemyDamageReceiver>();

            if (r == null)
            {
                continue;
            }

            Vector2 dir = (c.transform.position - playerPos).normalized;

            r.ApplyHit(damageOnHit, dir);

            applied = applied + 1;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, strikeRadius);
    }
}

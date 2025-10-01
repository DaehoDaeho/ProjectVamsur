using UnityEngine;

public enum MeleeStrikeMode
{
    Single,
    Area
}

/// <summary>
/// �ڵ� ���� ����(��� ��ȯ ����):
/// - Single: ���� ����� �� 1�� Ÿ��
/// - Area  : �ݰ� �� ���� �� Ÿ��(�ִ� �� ����)
/// </summary>
public class AutoMeleeStrikeFlex : MonoBehaviour
{
    [Header("Mode")]
    [SerializeField]
    private MeleeStrikeMode mode = MeleeStrikeMode.Single;

    [Header("Range")]
    [SerializeField]
    private float strikeRadius = 1.6f;            // [����] ����. Ÿ�� �ݰ�

    [SerializeField]
    private LayerMask enemyLayerMask;              // ��� ���̾�

    [Header("Timing")]
    [SerializeField]
    private float swingCooldownSeconds = 0.35f;    // [����] ��. �ߵ� �� �ּ� ����

    [Header("Damage")]
    [SerializeField]
    private int damageOnHit = 10;                  // [����] HP

    [Header("Area Mode Safety")]
    [SerializeField]
    private int maxTargetsPerStrike = 12;          // [����] Area ��忡�� �� ���� ó���� �ִ� ��

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

            float d = (c.transform.position - playerPos).magnitude; // [����] ����

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

        Vector2 dir = (best.position - playerPos).normalized; // [����] ���� ����

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

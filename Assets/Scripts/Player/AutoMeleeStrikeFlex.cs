using NUnit.Framework.Internal;
using UnityEngine;

public enum MeleeStrikeMode
{
    Single,
    Area
}

public class AutoMeleeStrikeFlex : MonoBehaviour
{
    [SerializeField]
    private MeleeStrikeMode mode = MeleeStrikeMode.Single;

    [SerializeField]
    private float strikeRadius = 1.5f;

    [SerializeField]
    private LayerMask enemyLayerMask;

    [SerializeField]
    private float swingCooldownSeconds = 0.4f;

    [SerializeField]
    private int damageOnHit = 10;

    [SerializeField]
    private int maxTargetsPerStrike = 10;

    [SerializeField]
    private float minKnockScale = 0.5f;

    private float cooldownTimer = 0.0f;
        
    // Update is called once per frame
    void Update()
    {
        if(cooldownTimer > 0.0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, strikeRadius, enemyLayerMask);

        if(hits == null)
        {
            return;
        }

        if(hits.Length == 0)
        {
            return;
        }

        if(cooldownTimer <= 0.0f)
        {
            if(mode == MeleeStrikeMode.Single)
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

    void StrikeSingle(Collider2D[] hits)
    {
        Transform best = null;
        float bestDist = float.MaxValue;
        Vector3 playerPos = transform.position;

        for(int i=0; i<hits.Length; ++i)
        {
            Collider2D c = hits[i];
            if (c == null)
            {
                continue;
            }

            float d = (c.transform.position - playerPos).magnitude;

            if(d < bestDist)
            {
                bestDist = d;
                best = c.transform;
            }
        }

        if(best == null)
        {
            return;
        }

        EnemyDamageReceiver r = best.GetComponent<EnemyDamageReceiver>();
        if(r != null)
        {
            Vector2 dir = (best.position - playerPos).normalized;
            r.ApplyHit(damageOnHit, dir);
        }
    }

    void StrikeArea(Collider2D[] hits)
    {
        Vector3 playerPos = transform.position;
        int applied = 0;

        for(int i=0; i<hits.Length; ++i)
        {
            if(applied >= maxTargetsPerStrike)
            {
                break;
            }

            Collider2D c = hits[i];
            if (c == null)
            {
                continue;
            }

            EnemyDamageReceiver r = c.GetComponent<EnemyDamageReceiver>();

            if(r == null)
            {
                continue;
            }

            float d = (c.transform.position - playerPos).magnitude; // 플레이어-적 거리.
            float t = 1.0f - Mathf.Clamp01(d / strikeRadius);   // 가중치 0(바깥)~1(가까움)
            float scale = 1.0f;

            scale = Mathf.Lerp(minKnockScale, 1.0f, t);

            Vector2 dir = (c.transform.position - playerPos).normalized * scale;
            r.ApplyHit(damageOnHit, dir);

            ++applied;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, strikeRadius);
    }
}

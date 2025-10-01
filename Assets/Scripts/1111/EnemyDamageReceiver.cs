using UnityEngine;

/// <summary>
/// 근접 피격 수신기:
/// - 대미지는 EnemyHealth.TakeDamage(...)에 위임.
/// - 살아있으면 '짧은 시간 linearVelocity 오버라이드 + 빠른 감쇠'로 넉백 적용.
/// - Rigidbody2D는 자신/부모/자식에서 안전 탐색.
/// </summary>
public class EnemyDamageReceiver : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    private EnemyHealth enemyHealth;

    private Rigidbody2D body;

    [Header("Knockback (Deterministic)")]
    [SerializeField]
    private float knockStartSpeed = 6.0f;       // [단위] 유닛/초. 넉백 시작 속도

    [SerializeField]
    private float knockDuration = 0.20f;        // [단위] 초. 오버라이드 유지 시간

    [SerializeField]
    private float knockDecayPerSecond = 25.0f;  // [단위] 유닛/초^2. 초당 줄일 속도(선형)

    [SerializeField]
    private float knockCooldownSeconds = 0.05f; // [단위] 초. 연속 넉백 최소 간격

    private Vector2 knockVel = Vector2.zero;
    private float knockTimer = 0.0f;
    private float knockCooldownTimer = 0.0f;

    private void Awake()
    {
        if (enemyHealth == null)
        {
            enemyHealth = GetComponent<EnemyHealth>();

            if (enemyHealth == null)
            {
                enemyHealth = GetComponentInParent<EnemyHealth>();
            }

            if (enemyHealth == null)
            {
                enemyHealth = GetComponentInChildren<EnemyHealth>();
            }
        }

        TryResolveBody();
    }

    private void Update()
    {
        // ===== 넉백 상태 갱신(속도 오버라이드 + 감쇠) =====
        if (knockTimer > 0.0f)
        {
            if (body != null)
            {
                body.linearVelocity = knockVel;
            }

            // [무엇] 선형 감쇠: v = max(0, v - a*dt)
            float reduce = knockDecayPerSecond * Time.deltaTime;

            if (knockVel.magnitude > reduce)
            {
                knockVel = knockVel.normalized * (knockVel.magnitude - reduce);
            }
            else
            {
                knockVel = Vector2.zero;
            }

            knockTimer = knockTimer - Time.deltaTime;

            if (knockTimer <= 0.0f)
            {
                if (body != null)
                {
                    body.linearVelocity = Vector2.zero;
                }

                knockVel = Vector2.zero;
            }
        }

        if (knockCooldownTimer > 0.0f)
        {
            knockCooldownTimer = knockCooldownTimer - Time.deltaTime;
        }
    }

    public void ApplyHit(int damage, Vector2 knockbackDir)
    {
        // 1) 대미지 위임(사망/파괴는 EnemyHealth가 처리)
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }

        // 파괴되었을 수 있으므로 즉시 안전성 확인
        if (this == null)
        {
            return;
        }

        if (enemyHealth == null)
        {
            return;
        }

        if (enemyHealth.IsDead() == true)
        {
            return;
        }

        // 2) 넉백 쿨다운(연속 튕김 보호)
        if (knockCooldownTimer > 0.0f)
        {
            return;
        }

        // 3) 방향 보정
        if (knockbackDir.sqrMagnitude < 0.0001f)
        {
            return;
        }

        Vector2 dir = knockbackDir.normalized;

        // 4) 결정론적 넉백 시작
        knockVel = dir * knockStartSpeed;
        knockTimer = knockDuration;
        knockCooldownTimer = knockCooldownSeconds;

        if (body == null)
        {
            TryResolveBody();
        }
    }

    private void TryResolveBody()
    {
        body = GetComponent<Rigidbody2D>();

        if (body == null)
        {
            body = GetComponentInParent<Rigidbody2D>();
        }

        if (body == null)
        {
            body = GetComponentInChildren<Rigidbody2D>();
        }

        if (body != null)
        {
            body.gravityScale = 0.0f;      // 탑다운
            body.freezeRotation = true;    // 회전 억제
        }
    }
}

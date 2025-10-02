using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 플레이어를 중심으로 반경 r로 계속 회전하는 칼(가시화 포함).
/// - SpriteRenderer 로 칼이 보임.
/// - 적을 맞추면 칼 색이 잠깐 플래시.
/// - 같은 적은 hitCooldownSeconds 동안 재피격 금지.
/// </summary>
public class OrbitBlade : MonoBehaviour
{
    [Header("Owner")]
    [SerializeField]
    private Transform playerTransform;

    [Header("Motion")]
    [SerializeField]
    private float radius = 1.2f;                 // [단위] 유닛

    [SerializeField]
    private float angularSpeedDeg = 180.0f;      // [단위] deg/s

    private float angleDeg = 0.0f;

    [Header("Damage")]
    [SerializeField]
    private int damageOnHit = 8;                 // [단위] HP

    [SerializeField]
    private float hitCooldownSeconds = 0.35f;    // [단위] 초

    [Header("Visual")]
    [SerializeField]
    private SpriteRenderer bladeRenderer;        // 칼 스프라이트

    [SerializeField]
    private Color bladeFlashColor = Color.yellow;

    [SerializeField]
    private float bladeFlashDuration = 0.08f;    // [단위] 초

    [SerializeField]
    private LayerMask enemyLayerMask;

    private readonly Dictionary<GameObject, float> lastHitTimeByTarget = new Dictionary<GameObject, float>();
    private Color bladeBaseColor;
    private float bladeFlashTimer = 0.0f;

    private void Awake()
    {
        if (bladeRenderer != null)
        {
            bladeBaseColor = bladeRenderer.color;
        }
    }

    private void Update()
    {
        if (playerTransform == null)
        {
            return;
        }

        // ===== 궤도 갱신 =====
        // [무엇] angle(t+dt) = angle(t) + ω*dt
        angleDeg = angleDeg + angularSpeedDeg * Time.deltaTime;

        float rad = angleDeg * Mathf.Deg2Rad;                           // [단위] rad = deg × π/180
        Vector2 offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;  // [무엇] (r cosθ, r sinθ)

        transform.position = (Vector2)playerTransform.position + offset;

        // ===== 칼 색 플래시 복귀 =====
        if (bladeFlashTimer > 0.0f)
        {
            bladeFlashTimer = bladeFlashTimer - Time.deltaTime;

            if (bladeFlashTimer <= 0.0f)
            {
                if (bladeRenderer != null)
                {
                    bladeRenderer.color = bladeBaseColor;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // [전제] 이 오브젝트에는 isTrigger=true Collider2D 가 있다.
        if (((1 << other.gameObject.layer) & enemyLayerMask) == 0)
        {
            return;
        }

        GameObject target = other.gameObject;

        // [무엇] 같은 적 재피격 최소 간격
        float lastTime;
        bool has = lastHitTimeByTarget.TryGetValue(target, out lastTime);

        if (has == true)
        {
            if (Time.time - lastTime < hitCooldownSeconds)
            {
                return;
            }
        }

        EnemyHealth health = target.GetComponent<EnemyHealth>();

        if (health == null)
        {
            return;
        }

        // ===== 피해 적용 =====
        health.TakeDamage(damageOnHit);
        lastHitTimeByTarget[target] = Time.time;

        // ===== 칼 플래시 =====
        if (bladeRenderer != null)
        {
            bladeRenderer.color = bladeFlashColor;
            bladeFlashTimer = bladeFlashDuration;
        }

        // ===== 적 플래시(맞은 대상) =====
        EnemyHitFlash flash = target.GetComponent<EnemyHitFlash>();

        if (flash != null)
        {
            flash.FlashOnce();
        }
    }
}

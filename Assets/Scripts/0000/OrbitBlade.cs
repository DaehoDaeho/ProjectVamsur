using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// �÷��̾ �߽����� �ݰ� r�� ��� ȸ���ϴ� Į(����ȭ ����).
/// - SpriteRenderer �� Į�� ����.
/// - ���� ���߸� Į ���� ��� �÷���.
/// - ���� ���� hitCooldownSeconds ���� ���ǰ� ����.
/// </summary>
public class OrbitBlade : MonoBehaviour
{
    [Header("Owner")]
    [SerializeField]
    private Transform playerTransform;

    [Header("Motion")]
    [SerializeField]
    private float radius = 1.2f;                 // [����] ����

    [SerializeField]
    private float angularSpeedDeg = 180.0f;      // [����] deg/s

    private float angleDeg = 0.0f;

    [Header("Damage")]
    [SerializeField]
    private int damageOnHit = 8;                 // [����] HP

    [SerializeField]
    private float hitCooldownSeconds = 0.35f;    // [����] ��

    [Header("Visual")]
    [SerializeField]
    private SpriteRenderer bladeRenderer;        // Į ��������Ʈ

    [SerializeField]
    private Color bladeFlashColor = Color.yellow;

    [SerializeField]
    private float bladeFlashDuration = 0.08f;    // [����] ��

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

        // ===== �˵� ���� =====
        // [����] angle(t+dt) = angle(t) + ��*dt
        angleDeg = angleDeg + angularSpeedDeg * Time.deltaTime;

        float rad = angleDeg * Mathf.Deg2Rad;                           // [����] rad = deg �� ��/180
        Vector2 offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;  // [����] (r cos��, r sin��)

        transform.position = (Vector2)playerTransform.position + offset;

        // ===== Į �� �÷��� ���� =====
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
        // [����] �� ������Ʈ���� isTrigger=true Collider2D �� �ִ�.
        if (((1 << other.gameObject.layer) & enemyLayerMask) == 0)
        {
            return;
        }

        GameObject target = other.gameObject;

        // [����] ���� �� ���ǰ� �ּ� ����
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

        // ===== ���� ���� =====
        health.TakeDamage(damageOnHit);
        lastHitTimeByTarget[target] = Time.time;

        // ===== Į �÷��� =====
        if (bladeRenderer != null)
        {
            bladeRenderer.color = bladeFlashColor;
            bladeFlashTimer = bladeFlashDuration;
        }

        // ===== �� �÷���(���� ���) =====
        EnemyHitFlash flash = target.GetComponent<EnemyHitFlash>();

        if (flash != null)
        {
            flash.FlashOnce();
        }
    }
}

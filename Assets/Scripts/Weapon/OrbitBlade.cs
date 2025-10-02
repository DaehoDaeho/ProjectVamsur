using UnityEngine;
using System.Collections.Generic;

public class OrbitBlade : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private float radius = 2.0f;

    [SerializeField]
    private float angularSpeedDeg = 180.0f;

    [SerializeField]
    private float damageOnHit = 10.0f;

    [SerializeField]
    private float hitCooldownSeconds = 0.3f;

    [SerializeField]
    private SpriteRenderer bladeRenderer;

    [SerializeField]
    private Color bladeFlashColor = Color.yellow;

    [SerializeField]
    private float bladeFlashDuratioin = 0.1f;

    [SerializeField]
    private LayerMask enemyLayerMask;

    private float angleDeg = 0.0f;

    private readonly Dictionary<GameObject, float> lastHitTimeByTarget = new Dictionary<GameObject, float>();
    private Color bladeBaseColor;
    private float bladeFlashTimer = 0.0f;

    private void Awake()
    {
        if(bladeRenderer != null)
        {
            bladeBaseColor = bladeRenderer.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform != null)
        {
            angleDeg = angleDeg + angularSpeedDeg * Time.deltaTime;

            float rad = angleDeg * Mathf.Deg2Rad;
            Vector2 offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;

            transform.position = (Vector2)playerTransform.position + offset;

            if(bladeFlashTimer > 0.0f)
            {
                bladeFlashTimer -= Time.deltaTime;
                if(bladeFlashTimer <= 0.0f)
                {
                    if(bladeRenderer != null)
                    {
                        bladeRenderer.color = bladeBaseColor;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(((1 << collision.gameObject.layer) & enemyLayerMask) == 0)
        {
            return;
        }

        GameObject target = collision.gameObject;

        float lastTime;
        bool has = lastHitTimeByTarget.TryGetValue(target, out lastTime);

        if(has == true)
        {
            if(Time.time - lastTime < hitCooldownSeconds)
            {
                return;
            }
        }

        EnemyHealth health = target.GetComponent<EnemyHealth>();
        if(health == null)
        {
            return;
        }

        health.TakeDamage(damageOnHit);
        lastHitTimeByTarget[target] = Time.time;

        if(bladeRenderer != null)
        {
            bladeRenderer.color = bladeFlashColor;
            bladeFlashTimer = bladeFlashDuratioin;
        }

        EnemyHitFlash flash = target.GetComponent<EnemyHitFlash>();
        if(flash != null)
        {
            flash.FlashOnce();
        }
    }
}

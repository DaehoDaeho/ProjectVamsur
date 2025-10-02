using UnityEngine;

/// <summary>
/// 적의 스프라이트 색을 잠깐 바꿔 '맞았다'는 시각 피드백 제공.
/// EnemyHealth.TakeDamage 이후나 무기에서 직접 호출해도 된다.
/// </summary>
public class EnemyHitFlash : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer targetRenderer;

    [SerializeField]
    private Color flashColor = new Color(1.0f, 0.4f, 0.4f, 1.0f);

    [SerializeField]
    private float flashDuration = 0.08f;        // [단위] 초

    private Color baseColor;
    private float timer = 0.0f;

    private void Awake()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<SpriteRenderer>();
        }

        if (targetRenderer != null)
        {
            baseColor = targetRenderer.color;
        }
    }

    private void Update()
    {
        if (timer > 0.0f)
        {
            timer = timer - Time.deltaTime;

            if (timer <= 0.0f)
            {
                if (targetRenderer != null)
                {
                    targetRenderer.color = baseColor;
                }
            }
        }
    }

    public void FlashOnce()
    {
        if (targetRenderer == null)
        {
            return;
        }

        targetRenderer.color = flashColor;
        timer = flashDuration;
    }
}

using UnityEngine;

public class EnemyHitFlash : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer targetRenderer;

    [SerializeField]
    private Color flashColor = Color.white;

    [SerializeField]
    private float flashDuration = 0.1f;

    private Color baseColor;
    private float timer = 0.0f;

    private void Awake()
    {
        if(targetRenderer != null)
        {
            baseColor = targetRenderer.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0.0f)
        {
            timer -= Time.deltaTime;
            if(timer <= 0.0f)
            {
                if(targetRenderer != null)
                {
                    targetRenderer.color = baseColor;
                }
            }
        }
    }

    public void FlashOnce()
    {
        if(targetRenderer != null)
        {
            targetRenderer.color = flashColor;
            timer = flashDuration;
        }
    }
}

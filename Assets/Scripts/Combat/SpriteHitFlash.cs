using UnityEngine;

public class SpriteHitFlash : MonoBehaviour
{
    [SerializeField]
    private Color flashColor = Color.white;

    [SerializeField]
    private float defaultFlashDuration = 0.05f;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private Color originalColor;
    private float flashEndTime = 0.0f;
    private bool isFlashing = false;

    private void Awake()
    {
        originalColor = spriteRenderer.color;
        isFlashing = false;
        flashEndTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(isFlashing == true)
        {
            if(Time.time < flashEndTime)
            {
                spriteRenderer.color = flashColor;
            }
            else
            {
                spriteRenderer.color = originalColor;
                isFlashing = false;
            }
        }
    }

    public void TriggerFlash(float duration)
    {
        float d = duration;

        if (d <= 0.0f)
        {
            d = defaultFlashDuration;
        }

        isFlashing = true;

        flashEndTime = Time.time + d;
    }
}

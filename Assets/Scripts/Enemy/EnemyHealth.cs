using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHp = 100.0f;

    private float currentHp = 0.0f;
    private bool isAlive = true;

    [SerializeField]
    private SpriteHitFlash spriteHitFlash;

    [SerializeField]
    private SimpleSound simpleSound;

    [SerializeField]
    private float flashDuration = 0.05f;

    [SerializeField]
    private SimpleHPUI enemyHP;

    [SerializeField]
    private PooledObject pooled;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitData();
    }

    private void OnEnable()
    {
        InitData();
    }

    void InitData()
    {
        currentHp = maxHp;
        isAlive = true;
        enemyHP.UpdateHPGage(currentHp, maxHp);
    }

    public void TakeDamage(float amount)
    {
        if(isAlive == true)
        {
            currentHp -= amount;

            if(spriteHitFlash != null)
            {
                spriteHitFlash.TriggerFlash(flashDuration);
            }

            if(simpleSound != null)
            {
                simpleSound.PlayHitAt(transform.position);
            }

            if(enemyHP != null)
            {
                enemyHP.UpdateHPGage(currentHp, maxHp);
            }

            if(currentHp <= 0.0f)
            {
                currentHp = 0.0f;
                isAlive = false;
                Die();
            }
        }
    }

    private void Die()
    {
        //Destroy(gameObject);
        pooled.ReturnToPool();
    }
}

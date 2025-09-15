using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField]
    private float damageAmount = 5.0f;

    [SerializeField]
    private float damageInterval = 0.5f;

    private float lastDamageTime = -9999.0f;

    // 충돌 상태가 지속중일 때 자동으로 호출되는 함수.
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") == true)
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            if(playerStats != null)
            {
                if(Time.time - lastDamageTime >= damageInterval)
                {
                    playerStats.TakeDamage(damageAmount);
                    lastDamageTime = Time.time;
                }
            }
        }
    }
}

using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField]
    private float damageAmount = 5.0f;

    [SerializeField]
    private float damageInterval = 0.5f;

    private float lastDamageTime = -9999.0f;

    // �浹 ���°� �������� �� �ڵ����� ȣ��Ǵ� �Լ�.
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

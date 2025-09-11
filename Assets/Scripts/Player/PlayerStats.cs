using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private float maxHp = 100.0f;

    private float currentHp = 0.0f;

    private bool isAlive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHp = maxHp;
        isAlive = true;
    }

    public void TakeDamage(float amount)
    {
        if(isAlive == true)
        {
            currentHp -= amount;
            if(currentHp <= 0.0f)
            {
                currentHp = 0.0f;
                isAlive = false;
                Debug.Log("플레이어 사망");
            }
        }
    }

    public bool GetIsAlive()
    {
        return isAlive;
    }

    public float GetCurrentHp()
    {
        return currentHp;
    }

    public float GetMaxHp()
    {
        return maxHp;
    }
}

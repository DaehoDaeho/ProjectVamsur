using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private float maxHp = 100.0f;

    [SerializeField]
    private SimpleHPUI simpleHPUI;

    private float currentHp = 0.0f;

    private bool isAlive = true;

    public event Action<float, float> OnChangedHP = null;

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
            Debug.Log("Take Damage!!!!!");
            currentHp -= amount;
            if(currentHp <= 0.0f)
            {
                currentHp = 0.0f;
                isAlive = false;
                Debug.Log("플레이어 사망");
            }

            if(OnChangedHP != null)
            {
                OnChangedHP.Invoke(currentHp, maxHp);
            }

            if(simpleHPUI != null)
            {
                simpleHPUI.UpdateHPGage(currentHp, maxHp);
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

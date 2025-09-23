using UnityEngine;
using UnityEngine.UI;

public class SimpleHPUI : MonoBehaviour
{
    [SerializeField]
    private Image imageHP;

    public void UpdateHPGage(float currentHP, float maxHP)
    {
        if (currentHP <= 0.0f)
        {
            imageHP.fillAmount = 0.0f;
            return;
        }

        float value = currentHP / maxHP;
        imageHP.fillAmount = value;
    }
}

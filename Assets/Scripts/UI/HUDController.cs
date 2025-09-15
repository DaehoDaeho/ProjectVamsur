using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField]
    private Image imageHPFill;

    [SerializeField]
    private PlayerStats playerStats;

    private void OnEnable()
    {
        playerStats.OnChangedHP += UpdateHPGage;
    }

    void UpdateHPGage(float current, float max)
    {
        float value = current / max;
        imageHPFill.fillAmount = value;
    }
}

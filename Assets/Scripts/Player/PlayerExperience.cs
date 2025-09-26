using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerExperience : MonoBehaviour
{
    [SerializeField]
    private int level = 1;

    [SerializeField]
    private int xpCurrent = 0;

    [SerializeField]
    private int xpForNext = 5;

    [SerializeField]
    private int xpIncreasePerLevel = 3;

    [SerializeField]
    private Image xpFillImage;

    [SerializeField]
    private TMP_Text textXp;

    private void Start()
    {
        UpdateBar();
    }

    public void AddXP(int amount)
    {
        xpCurrent += amount;
        UpdateBar();

        while(xpCurrent >= xpForNext)
        {
            xpCurrent = xpCurrent - xpForNext;
            level = level + 1;
            xpForNext = xpForNext + xpIncreasePerLevel;
            UpdateBar();
        }
    }

    void UpdateBar()
    {
        if(xpFillImage != null)
        {
            float fill = 0.0f;

            if(xpForNext > 0.0f)
            {
                fill = (float)xpCurrent / (float)xpForNext;
            }

            if(fill < 0.0f)
            {
                fill = 0.0f;
            }

            if(fill > 1.0f)
            {
                fill = 1.0f;
            }

            xpFillImage.fillAmount = fill;
        }

        if(textXp != null)
        {
            textXp.text = "Level: " + level.ToString();
        }
    }

    public int GetLevel()
    {
        return level;
    }

    public int GetCurrentXP()
    {
        return xpCurrent;
    }

    public int GetNextXpTarget()
    {
        return xpForNext;
    }
}

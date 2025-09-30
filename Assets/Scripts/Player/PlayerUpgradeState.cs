using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어가 어떤 옵션을 몇 레벨까지 올렸는지 보관/조회.
/// - 키: optionId (SO의 식별자)
/// - 값: 현재 레벨(1,2,...)
/// </summary>
public class PlayerUpgradeState : MonoBehaviour
{
    private readonly Dictionary<string, int> levelByOptionId = new Dictionary<string, int>();

    public int GetLevel(string optionId)
    {
        if (levelByOptionId.TryGetValue(optionId, out int lv) == true)
        {
            return lv;
        }

        return 0;
    }

    public void AddLevel(string optionId, int maxLevel)
    {
        int lv = GetLevel(optionId);

        if (lv < maxLevel)
        {
            lv = lv + 1;
            levelByOptionId[optionId] = lv;
        }
    }

    public bool IsMaxed(LevelUpOptionSO o)
    {
        if (o == null)
        {
            return true;
        }

        int lv = GetLevel(o.optionId);

        if (lv >= o.maxLevel)
        {
            return true;
        }

        return false;
    }
}

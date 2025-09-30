using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾ � �ɼ��� �� �������� �÷ȴ��� ����/��ȸ.
/// - Ű: optionId (SO�� �ĺ���)
/// - ��: ���� ����(1,2,...)
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

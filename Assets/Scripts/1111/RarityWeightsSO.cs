using UnityEngine;

public enum UpgradeRarity
{
    Common,
    Uncommon,
    Rare,
    Epic
}


/// <summary>
/// ��͵��� ����ġ ����(�����̳ʰ� ���¿��� ����).
/// ��: Common=60, Uncommon=30, Rare=9, Epic=1
/// </summary>
[CreateAssetMenu(fileName = "RarityWeights", menuName = "Game/Upgrade/RarityWeights")]
public class RarityWeightsSO : ScriptableObject
{
    public int weightCommon = 60;
    public int weightUncommon = 30;
    public int weightRare = 9;
    public int weightEpic = 1;

    public int GetWeight(UpgradeRarity r)
    {
        if (r == UpgradeRarity.Common)
        {
            return weightCommon;
        }

        if (r == UpgradeRarity.Uncommon)
        {
            return weightUncommon;
        }

        if (r == UpgradeRarity.Rare)
        {
            return weightRare;
        }

        // Epic
        return weightEpic;
    }
}

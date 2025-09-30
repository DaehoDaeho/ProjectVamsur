using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ �� ������ '���� �ٸ� 3�� �ɼ�'�� �̴´�.
/// - �ܰ�1: ��͵��� ����ġ �տ��� ��÷(�귿�� ���).
/// - �ܰ�2: �� ��͵� ������ ���� '�ִ뷹�� �̸�'�� ī�� �� �ϳ��� ���� ����.
/// - �ܰ�3: ���� ������ ���ÿ� �ߺ� ī�� ����(�̹� ���� optionId ����).
/// </summary>
public class LevelUpPicker : MonoBehaviour
{
    [SerializeField]
    private RarityWeightsSO rarityWeights;

    [SerializeField]
    private List<LevelUpOptionSO> optionDatabase;

    [SerializeField]
    private PlayerUpgradeState playerState;

    public bool TryPickUniqueOptions(int count, List<LevelUpOptionSO> outList)
    {
        if (outList == null)
        {
            return false;
        }

        outList.Clear();

        if (rarityWeights == null)
        {
            return false;
        }

        if (optionDatabase == null)
        {
            return false;
        }

        if (playerState == null)
        {
            return false;
        }

        // [����] �̹� ������ ���ÿ��� �̹� ���� optionId ����(�ߺ� ����).
        HashSet<string> used = new HashSet<string>();

        // [����] �ִ� 'count'�� �̱� �õ�.
        for (int i = 0; i < count; i = i + 1)
        {
            LevelUpOptionSO pick = PickOne(used);

            if (pick != null)
            {
                outList.Add(pick);
                used.Add(pick.optionId);
            }
        }

        if (outList.Count > 0)
        {
            return true;
        }

        return false;
    }

    private LevelUpOptionSO PickOne(HashSet<string> used)
    {
        // ===== 1) ��͵� �귿 �� =====
        // [����] �� ��͵��� ����ġ ���� �����.
        // [��] 0~�� ���� ���ǰ��� �̾� �ش� ������ �ɸ� ��͵��� �����ϱ� ����.
        int wCommon = rarityWeights.GetWeight(UpgradeRarity.Common);
        int wUncommon = rarityWeights.GetWeight(UpgradeRarity.Uncommon);
        int wRare = rarityWeights.GetWeight(UpgradeRarity.Rare);
        int wEpic = rarityWeights.GetWeight(UpgradeRarity.Epic);

        int sum = wCommon + wUncommon + wRare + wEpic;

        if (sum <= 0)
        {
            return null;
        }

        int t = Random.Range(0, sum); // [����] ����. ���� ����, ���� ����.

        UpgradeRarity selected = UpgradeRarity.Common;

        if (t < wCommon)
        {
            selected = UpgradeRarity.Common;
        }
        else if (t < wCommon + wUncommon)
        {
            selected = UpgradeRarity.Uncommon;
        }
        else if (t < wCommon + wUncommon + wRare)
        {
            selected = UpgradeRarity.Rare;
        }
        else
        {
            selected = UpgradeRarity.Epic;
        }

        // ===== 2) ��͵� ������ �ĺ� ���͸� =====
        // [����] selected ��͵��̰�, ���� �ִ뷹�� �̸��̸�, �̹� ���ÿ� ���� ������ ���� �ɼǸ� �ĺ�.
        List<LevelUpOptionSO> candidates = new List<LevelUpOptionSO>();

        for (int i = 0; i < optionDatabase.Count; i = i + 1)
        {
            LevelUpOptionSO o = optionDatabase[i];

            if (o == null)
            {
                continue;
            }

            if (o.rarity != selected)
            {
                continue;
            }

            if (playerState.IsMaxed(o) == true)
            {
                continue;
            }

            if (used.Contains(o.optionId) == true)
            {
                continue;
            }

            candidates.Add(o);
        }

        // [����] �ĺ��� ����ٸ�, ��͵� ������ �� �ܰ� ��ȭ�� ����(����).
        // [��] Rare/Epic�� �� �����̸� ������ �� �� �� �����Ƿ� Common���� Ȯ���� �� ���� ����.
        if (candidates.Count == 0)
        {
            for (int i = 0; i < optionDatabase.Count; i = i + 1)
            {
                LevelUpOptionSO o = optionDatabase[i];

                if (o == null)
                {
                    continue;
                }

                if (playerState.IsMaxed(o) == true)
                {
                    continue;
                }

                if (used.Contains(o.optionId) == true)
                {
                    continue;
                }

                candidates.Add(o);
            }
        }

        if (candidates.Count == 0)
        {
            return null;
        }

        // ===== 3) �ĺ� �� �ϳ� ���� =====
        int idx = Random.Range(0, candidates.Count);

        return candidates[idx];
    }
}

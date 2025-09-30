using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 레벨업 때 제시할 '서로 다른 3개 옵션'을 뽑는다.
/// - 단계1: 희귀도를 가중치 합에서 추첨(룰렛휠 방식).
/// - 단계2: 그 희귀도 내에서 아직 '최대레벨 미만'인 카드 중 하나를 랜덤 선택.
/// - 단계3: 같은 레벨업 제시에 중복 카드 금지(이미 뽑힌 optionId 제외).
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

        // [무엇] 이번 레벨업 제시에서 이미 뽑은 optionId 집합(중복 방지).
        HashSet<string> used = new HashSet<string>();

        // [무엇] 최대 'count'번 뽑기 시도.
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
        // ===== 1) 희귀도 룰렛 휠 =====
        // [무엇] 각 희귀도의 가중치 합을 만든다.
        // [왜] 0~합 사이 임의값을 뽑아 해당 구간에 걸린 희귀도를 선택하기 위함.
        int wCommon = rarityWeights.GetWeight(UpgradeRarity.Common);
        int wUncommon = rarityWeights.GetWeight(UpgradeRarity.Uncommon);
        int wRare = rarityWeights.GetWeight(UpgradeRarity.Rare);
        int wEpic = rarityWeights.GetWeight(UpgradeRarity.Epic);

        int sum = wCommon + wUncommon + wRare + wEpic;

        if (sum <= 0)
        {
            return null;
        }

        int t = Random.Range(0, sum); // [단위] 정수. 하한 포함, 상한 제외.

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

        // ===== 2) 희귀도 내에서 후보 필터링 =====
        // [무엇] selected 희귀도이고, 아직 최대레벨 미만이며, 이번 제시에 아직 사용되지 않은 옵션만 후보.
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

        // [무엇] 후보가 비었다면, 희귀도 조건을 한 단계 완화해 본다(폴백).
        // [왜] Rare/Epic이 다 만렙이면 선택을 못 할 수 있으므로 Common까지 확장해 빈 손을 방지.
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

        // ===== 3) 후보 중 하나 랜덤 =====
        int idx = Random.Range(0, candidates.Count);

        return candidates[idx];
    }
}

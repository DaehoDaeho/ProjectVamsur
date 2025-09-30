using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelUpPopup : MonoBehaviour
{
    [Header("Database & State")]
    [SerializeField]
    private List<LevelUpOptionSO> optionDatabase;

    [SerializeField]
    private RarityWeightsSO rarityWeights;                 // 희귀도 가중치 에셋

    [SerializeField]
    private PlayerUpgradeState playerState;                // 플레이어 업그레이드 보유 레벨 상태

    [Header("UI")]
    [SerializeField]
    private GameObject panelRoot;

    [SerializeField]
    private Button[] button;                               // 3개 버튼

    [SerializeField]
    private Text[] title;                                  // 3개 제목

    [SerializeField]
    private Text[] desc;                                   // 3개 설명

    [SerializeField]
    private Button rerollButton;                           // 리롤(선택) 버튼

    [Header("Apply Target")]
    [SerializeField]
    private PlayerUpgradeApplier applier;

    [SerializeField]
    private PlayerExperience exp;

    private LevelUpOptionSO[] pick = new LevelUpOptionSO[3];

    private float prevTimeScale = 1.0f;

    private bool rerolledThisLevel = false;               // 이번 레벨업에서 이미 리롤했는가

    void Start()
    {
        HideImmediate();
        BindToPlayer();

        if (rerollButton != null)
        {
            rerollButton.onClick.RemoveAllListeners();
            rerollButton.onClick.AddListener(HandleReroll);
        }
    }

    public void BindToPlayer()
    {
        if (exp != null)
        {
            // 주의: 너의 기존 이벤트 이름을 그대로 사용(OnLevelUp)
            exp.OnLevelUp += OnPlayerLevelUp;
        }
    }

    void OnPlayerLevelUp(int newLevel)
    {
        rerolledThisLevel = false;
        Show();
    }

    public void Show()
    {
        // ===== 안전성 체크 =====
        if (optionDatabase == null)
        {
            return;
        }

        if (optionDatabase.Count == 0)
        {
            return;
        }

        // [무엇] 이번 팝업에서 '서로 다른 3장'을 뽑을 때 중복을 막기 위한 set.
        HashSet<string> used = new HashSet<string>();

        // [무엇] 3장 뽑기(가중치 희귀도 → 유효 카드 랜덤) 시도.
        int filled = 0;

        for (int i = 0; i < pick.Length; i = i + 1)
        {
            LevelUpOptionSO p = PickOneUnique(used);

            if (p != null)
            {
                pick[i] = p;

                // [무엇] 중복 방지 키 = optionId가 있으면 그걸 사용. 없으면 title로 폴백.
                string key = string.IsNullOrEmpty(p.optionId) == false ? p.optionId : p.title;

                used.Add(key);

                filled = filled + 1;
            }
            else
            {
                pick[i] = null;
            }
        }

        if (filled == 0)
        {
            // [무엇] 제시할 카드가 전혀 없으면(모두 만렙 등) 그냥 스킵.
            return;
        }

        // ===== UI 바인딩 =====
        for (int i = 0; i < pick.Length; i = i + 1)
        {
            if (title != null && i < title.Length && title[i] != null)
            {
                title[i].text = pick[i] != null ? pick[i].title : "-";
            }
        }

        for (int i = 0; i < pick.Length; i = i + 1)
        {
            if (desc != null && i < desc.Length && desc[i] != null)
            {
                desc[i].text = pick[i] != null ? pick[i].description : "";
            }
        }

        // 버튼 콜백
        if (button != null && button.Length >= 3)
        {
            if (button[0] != null)
            {
                button[0].onClick.RemoveAllListeners();
                button[0].onClick.AddListener(OnPick1);
                button[0].interactable = (pick[0] != null);
            }

            if (button[1] != null)
            {
                button[1].onClick.RemoveAllListeners();
                button[1].onClick.AddListener(OnPick2);
                button[1].interactable = (pick[1] != null);
            }

            if (button[2] != null)
            {
                button[2].onClick.RemoveAllListeners();
                button[2].onClick.AddListener(OnPick3);
                button[2].interactable = (pick[2] != null);
            }
        }

        // [무엇] 타임스케일 정지.
        prevTimeScale = 1.0f;
        Time.timeScale = 0.0f;

        if (panelRoot != null)
        {
            panelRoot.SetActive(true);
        }

        // [무엇] 리롤 버튼 노출(이번 레벨업에서 아직 리롤하지 않았다면).
        if (rerollButton != null)
        {
            rerollButton.gameObject.SetActive(rerolledThisLevel == false);
            rerollButton.interactable = (filled >= 1);
        }
    }

    // ===== 유니크 카드 1장 뽑기 =====
    private LevelUpOptionSO PickOneUnique(HashSet<string> used)
    {
        // [무엇] 희귀도 가중치 합 계산.
        int wCommon = rarityWeights != null ? rarityWeights.GetWeight(UpgradeRarity.Common) : 1;
        int wUncommon = rarityWeights != null ? rarityWeights.GetWeight(UpgradeRarity.Uncommon) : 1;
        int wRare = rarityWeights != null ? rarityWeights.GetWeight(UpgradeRarity.Rare) : 1;
        int wEpic = rarityWeights != null ? rarityWeights.GetWeight(UpgradeRarity.Epic) : 1;

        int sum = wCommon + wUncommon + wRare + wEpic;

        if (sum <= 0)
        {
            sum = 1;
        }

        int t = Random.Range(0, sum); // [무엇] t ~ U[0, sum)

        // [무엇] 누적 구간 비교로 희귀도 선택.
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

        // [무엇] 1차 후보: 선택된 희귀도 ∧ 만렙 아님 ∧ 이번 제시에 미사용.
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

            if (IsMaxed(o) == true)
            {
                continue;
            }

            string key = string.IsNullOrEmpty(o.optionId) == false ? o.optionId : o.title;

            if (used.Contains(key) == true)
            {
                continue;
            }

            candidates.Add(o);
        }

        // [무엇] 폴백: 같은 기준으로 전체 희귀도에서 탐색(희귀도 구속력 완화).
        if (candidates.Count == 0)
        {
            for (int i = 0; i < optionDatabase.Count; i = i + 1)
            {
                LevelUpOptionSO o = optionDatabase[i];

                if (o == null)
                {
                    continue;
                }

                if (IsMaxed(o) == true)
                {
                    continue;
                }

                string key = string.IsNullOrEmpty(o.optionId) == false ? o.optionId : o.title;

                if (used.Contains(key) == true)
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

        int idx = Random.Range(0, candidates.Count);

        return candidates[idx];
    }

    private bool IsMaxed(LevelUpOptionSO o)
    {
        if (playerState == null)
        {
            // [전제] 상태 관리가 없으면 만렙 제외 기능을 비활성화.
            return false;
        }

        if (o == null)
        {
            return true;
        }

        // [무엇] 상태에 기록된 현재 레벨이 maxLevel 이상이면 만렙.
        if (playerState.IsMaxed(o) == true)
        {
            return true;
        }

        return false;
    }

    // ===== 리롤 처리 =====
    private void HandleReroll()
    {
        if (rerolledThisLevel == false)
        {
            rerolledThisLevel = true;
            // [무엇] 새로 뽑아 UI 갱신(타임스케일 0 유지).
            Show();
        }
    }

    void OnPick1()
    {
        ApplyAndClose(pick[0]);
    }

    void OnPick2()
    {
        ApplyAndClose(pick[1]);
    }

    void OnPick3()
    {
        ApplyAndClose(pick[2]);
    }

    void ApplyAndClose(LevelUpOptionSO picked)
    {
        if (applier != null)
        {
            applier.Apply(picked);
        }

        Close();
    }

    void Close()
    {
        HideImmediate();

        Time.timeScale = prevTimeScale;
    }

    void HideImmediate()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(false);
        }
    }
}

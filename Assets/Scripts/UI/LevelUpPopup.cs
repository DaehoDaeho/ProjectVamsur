using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelUpPopup : MonoBehaviour
{
    [Header("Database & State")]
    [SerializeField]
    private List<LevelUpOptionSO> optionDatabase;

    [SerializeField]
    private RarityWeightsSO rarityWeights;                 // ��͵� ����ġ ����

    [SerializeField]
    private PlayerUpgradeState playerState;                // �÷��̾� ���׷��̵� ���� ���� ����

    [Header("UI")]
    [SerializeField]
    private GameObject panelRoot;

    [SerializeField]
    private Button[] button;                               // 3�� ��ư

    [SerializeField]
    private Text[] title;                                  // 3�� ����

    [SerializeField]
    private Text[] desc;                                   // 3�� ����

    [SerializeField]
    private Button rerollButton;                           // ����(����) ��ư

    [Header("Apply Target")]
    [SerializeField]
    private PlayerUpgradeApplier applier;

    [SerializeField]
    private PlayerExperience exp;

    private LevelUpOptionSO[] pick = new LevelUpOptionSO[3];

    private float prevTimeScale = 1.0f;

    private bool rerolledThisLevel = false;               // �̹� ���������� �̹� �����ߴ°�

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
            // ����: ���� ���� �̺�Ʈ �̸��� �״�� ���(OnLevelUp)
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
        // ===== ������ üũ =====
        if (optionDatabase == null)
        {
            return;
        }

        if (optionDatabase.Count == 0)
        {
            return;
        }

        // [����] �̹� �˾����� '���� �ٸ� 3��'�� ���� �� �ߺ��� ���� ���� set.
        HashSet<string> used = new HashSet<string>();

        // [����] 3�� �̱�(����ġ ��͵� �� ��ȿ ī�� ����) �õ�.
        int filled = 0;

        for (int i = 0; i < pick.Length; i = i + 1)
        {
            LevelUpOptionSO p = PickOneUnique(used);

            if (p != null)
            {
                pick[i] = p;

                // [����] �ߺ� ���� Ű = optionId�� ������ �װ� ���. ������ title�� ����.
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
            // [����] ������ ī�尡 ���� ������(��� ���� ��) �׳� ��ŵ.
            return;
        }

        // ===== UI ���ε� =====
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

        // ��ư �ݹ�
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

        // [����] Ÿ�ӽ����� ����.
        prevTimeScale = 1.0f;
        Time.timeScale = 0.0f;

        if (panelRoot != null)
        {
            panelRoot.SetActive(true);
        }

        // [����] ���� ��ư ����(�̹� ���������� ���� �������� �ʾҴٸ�).
        if (rerollButton != null)
        {
            rerollButton.gameObject.SetActive(rerolledThisLevel == false);
            rerollButton.interactable = (filled >= 1);
        }
    }

    // ===== ����ũ ī�� 1�� �̱� =====
    private LevelUpOptionSO PickOneUnique(HashSet<string> used)
    {
        // [����] ��͵� ����ġ �� ���.
        int wCommon = rarityWeights != null ? rarityWeights.GetWeight(UpgradeRarity.Common) : 1;
        int wUncommon = rarityWeights != null ? rarityWeights.GetWeight(UpgradeRarity.Uncommon) : 1;
        int wRare = rarityWeights != null ? rarityWeights.GetWeight(UpgradeRarity.Rare) : 1;
        int wEpic = rarityWeights != null ? rarityWeights.GetWeight(UpgradeRarity.Epic) : 1;

        int sum = wCommon + wUncommon + wRare + wEpic;

        if (sum <= 0)
        {
            sum = 1;
        }

        int t = Random.Range(0, sum); // [����] t ~ U[0, sum)

        // [����] ���� ���� �񱳷� ��͵� ����.
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

        // [����] 1�� �ĺ�: ���õ� ��͵� �� ���� �ƴ� �� �̹� ���ÿ� �̻��.
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

        // [����] ����: ���� �������� ��ü ��͵����� Ž��(��͵� ���ӷ� ��ȭ).
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
            // [����] ���� ������ ������ ���� ���� ����� ��Ȱ��ȭ.
            return false;
        }

        if (o == null)
        {
            return true;
        }

        // [����] ���¿� ��ϵ� ���� ������ maxLevel �̻��̸� ����.
        if (playerState.IsMaxed(o) == true)
        {
            return true;
        }

        return false;
    }

    // ===== ���� ó�� =====
    private void HandleReroll()
    {
        if (rerolledThisLevel == false)
        {
            rerolledThisLevel = true;
            // [����] ���� �̾� UI ����(Ÿ�ӽ����� 0 ����).
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

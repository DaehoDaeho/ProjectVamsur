using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelUpPopup : MonoBehaviour
{
    [SerializeField]
    private List<LevelUpOptionSO> optionDatabase;

    [SerializeField]
    private GameObject panelRoot;

    [SerializeField]
    private Button[] button;

    [SerializeField]
    private Text[] title;

    [SerializeField]
    private Text[] desc;

    [SerializeField]
    private PlayerUpgradeApplier applier;

    [SerializeField]
    private PlayerExperience exp;

    private LevelUpOptionSO[] pick = new LevelUpOptionSO[3];

    private float prevTimeScale = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideImmediate();
        BindToPlayer();
    }

    public void BindToPlayer()
    {
        if(exp != null)
        {
            exp.OnLevelUp += OnPlayerLevelUp;
        }
    }

    void OnPlayerLevelUp(int newLevel)
    {
        Show();
    }


    public void Show()
    {
        if(optionDatabase == null)
        {
            return;
        }

        if(optionDatabase.Count < 3)
        {
            return;
        }

        int a = Random.Range(0, optionDatabase.Count);
        int b = Random.Range(0, optionDatabase.Count);

        while(a == b)
        {
            b = Random.Range(0, optionDatabase.Count);
        }

        int c = Random.Range(0, optionDatabase.Count);

        while(a == c || b == c)
        {
            c = Random.Range(0, optionDatabase.Count);
        }

        pick[0] = optionDatabase[a];
        pick[1] = optionDatabase[b];
        pick[2] = optionDatabase[c];

        for(int i=0; i< pick.Length; ++i)
        {
            title[i].text = pick[i].title;
        }

        for (int i = 0; i < pick.Length; ++i)
        {
            desc[i].text = pick[i].description;
        }

        if (button[0] != null)
        {
            button[0].onClick.RemoveAllListeners();
            button[0].onClick.AddListener(OnPick1);
        }

        if (button[1] != null)
        {
            button[1].onClick.RemoveAllListeners();
            button[1].onClick.AddListener(OnPick2);
        }

        if (button[2] != null)
        {
            button[2].onClick.RemoveAllListeners();
            button[2].onClick.AddListener(OnPick3);
        }

        prevTimeScale = Time.timeScale;
        Time.timeScale = 0.0f;

        if(panelRoot != null)
        {
            panelRoot.SetActive(true);
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

    void ApplyAndClose(LevelUpOptionSO pick)
    {
        if(applier != null)
        {
            applier.Apply(pick);
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
        if(panelRoot != null)
        {
            panelRoot.SetActive(false);
        }
    }
}

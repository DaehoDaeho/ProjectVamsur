using UnityEngine;

/// <summary>
/// 스칼라를 읽어 '동시 최대 적 수'와 '틱당 스폰량'으로 환산해 주기적 적용.
/// - 0.5초 간격 갱신, 정수 내림으로 체감 튐 억제.
/// </summary>
public class DifficultyDriver : MonoBehaviour
{
    [SerializeField]
    private DifficultyScalar difficultyScalar;

    [SerializeField]
    private SpawnerEnemyRing spawner;

    [Header("Base Values")]
    [SerializeField]
    private int baseMaxAlive = 20;

    [SerializeField]
    private int baseSpawnsPerTick = 1;

    [Header("Hard Caps")]
    [SerializeField]
    private int hardCapMaxAlive = 80;

    [SerializeField]
    private int hardCapSpawnsPerTick = 3;

    [Header("Update Interval")]
    [SerializeField]
    private float updateIntervalSeconds = 0.5f;

    private float lastUpdateTime = -9999.0f;

    private void Update()
    {
        if (difficultyScalar == null)
        {
            return;
        }

        if (spawner == null)
        {
            return;
        }

        if (Time.time - lastUpdateTime < updateIntervalSeconds)
        {
            return;
        }

        lastUpdateTime = Time.time;

        float s = difficultyScalar.GetScalar(); // [무엇] 난이도 스칼라(무차원).

        int maxAlive = Mathf.FloorToInt(baseMaxAlive * s); // [무엇] 동시 상한.
        if (maxAlive > hardCapMaxAlive)
        {
            maxAlive = hardCapMaxAlive;
        }
        if (maxAlive < 1)
        {
            maxAlive = 1;
        }

        int spt = Mathf.FloorToInt(baseSpawnsPerTick * (0.7f + 0.3f * s)); // [무엇] 틱당 스폰(완만 가속).
        if (spt > hardCapSpawnsPerTick)
        {
            spt = hardCapSpawnsPerTick;
        }
        if (spt < 1)
        {
            spt = 1;
        }

        spawner.SetDynamicMaxAlive(maxAlive);
        spawner.SetDynamicSpawnsPerTick(spt);
    }
}

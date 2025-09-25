using UnityEngine;

/// <summary>
/// ��Į�� �о� '���� �ִ� �� ��'�� 'ƽ�� ������'���� ȯ���� �ֱ��� ����.
/// - 0.5�� ���� ����, ���� �������� ü�� Ʀ ����.
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

        float s = difficultyScalar.GetScalar(); // [����] ���̵� ��Į��(������).

        int maxAlive = Mathf.FloorToInt(baseMaxAlive * s); // [����] ���� ����.
        if (maxAlive > hardCapMaxAlive)
        {
            maxAlive = hardCapMaxAlive;
        }
        if (maxAlive < 1)
        {
            maxAlive = 1;
        }

        int spt = Mathf.FloorToInt(baseSpawnsPerTick * (0.7f + 0.3f * s)); // [����] ƽ�� ����(�ϸ� ����).
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

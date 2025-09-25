using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class SpawnerEnemyRing : MonoBehaviour
{
    public enum SpawnMode
    {
        PlayerRing,
        CameraEdge,
    }

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private Camera targetCamera;

    [SerializeField]
    //private GameObject enemyPrefab;
    private PrefabPool enemyPrefabPool;

    [SerializeField]
    private float spawnIntervalSeconds = 1.0f;

    [SerializeField]
    private int spawnsPerTick = 1;

    private float lastSpawnTime = -9999.0f;

    [SerializeField]
    private int maxAlive = 30;

    private List<GameObject> aliveList = new List<GameObject>();

    [SerializeField]
    private SpawnMode spawnMode = SpawnMode.PlayerRing;

    [SerializeField]
    private float spawnRadius = 10.0f;

    [SerializeField]
    private float cameraEdgePadding = 1.5f;

    [SerializeField]
    private float randomAngleJitterDegrees = 10.0f;

    [SerializeField]
    private LayerMask envitonmentMask;

    [SerializeField]
    private float overlapCheckRadius = 0.4f;

    [SerializeField]
    private int maxPlacementTries = 5;
        
    // Update is called once per frame
    void Update()
    {
        if(Time.time -lastSpawnTime >= spawnIntervalSeconds)
        {
            TrySpawnTick();
            lastSpawnTime = Time.time;
        }
    }

    void TrySpawnTick()
    {
        ClearNullsFromAliveList();

        int aliveCount = aliveList.Count;

        if(aliveCount >= maxAlive)
        {
            return;
        }

        int remainingSlots = maxAlive - aliveCount;

        int spawnThisTick = spawnsPerTick;

        if(remainingSlots < spawnThisTick)
        {
            spawnThisTick = remainingSlots;
        }

        for(int i=0; i<spawnThisTick; ++i)
        {
            Vector3 pos;

            bool found = TryFindSpawnPosition(out pos);

            if (found == true)
            {
                //GameObject go = Instantiate(enemyPrefab, pos, Quaternion.identity);
                GameObject go = enemyPrefabPool.Get(pos, Quaternion.identity);

                if(go == null)
                {
                    return;
                }

                EnemySimple enemySimple = go.GetComponent<EnemySimple>();
                if (enemySimple != null)
                {
                    enemySimple.SetTarget(playerTransform);
                }

                aliveList.Add(go);
            }
        }
    }

    void ClearNullsFromAliveList()
    {
        for(int i=aliveList.Count-1; i>=0; --i)
        {
            //if (aliveList[i] == null)
            if (aliveList[i].activeSelf == false)
            {
                aliveList.RemoveAt(i);
            }
        }
    }

    bool TryFindSpawnPosition(out Vector3 result)
    {
        for(int tries = 0; tries < maxPlacementTries; ++tries)
        {
            Vector3 candidate;

            if (spawnMode == SpawnMode.PlayerRing)
            {
                float angleDeg = Random.Range(0.0f, 360.0f);

                float jitter = Random.Range(-randomAngleJitterDegrees, randomAngleJitterDegrees);

                float finalDeg = angleDeg + jitter;

                float rad = finalDeg * Mathf.Deg2Rad;

                float dx = Mathf.Cos(rad);
                float dy = Mathf.Sin(rad);

                Vector2 dir = new Vector2(dx, dy).normalized;

                if (playerTransform != null)
                {
                    candidate = playerTransform.position + new Vector3(dir.x, dir.y, 0.0f) * spawnRadius;
                }
                else
                {
                    candidate = transform.position + new Vector3(dir.x, dir.y, 0.0f) * spawnRadius;
                }
            }
            else
            {
                if(targetCamera == null)
                {
                    result = Vector3.zero;
                    return false;
                }

                float halfHeight = targetCamera.orthographicSize;
                float halfWidth = halfHeight * targetCamera.aspect;

                int edge = Random.Range(0, 4);

                Vector3 camPos = targetCamera.transform.position;

                if(edge == 0)
                {
                    float y = camPos.y + halfHeight + cameraEdgePadding;
                    float x = Random.Range(camPos.x - halfWidth, camPos.x + halfWidth);
                    candidate = new Vector3(x, y, 0.0f);
                }
                else if (edge == 1)
                {
                    float y = camPos.y - halfHeight - cameraEdgePadding;
                    float x = Random.Range(camPos.x - halfWidth, camPos.x + halfWidth);
                    candidate = new Vector3(x, y, 0.0f);
                }
                else if(edge == 2)
                {
                    float x = camPos.x - halfWidth - cameraEdgePadding;
                    float y = Random.Range(camPos.y - halfHeight, camPos.y + halfHeight);
                    candidate = new Vector3(x, y, 0.0f);
                }
                else
                {
                    float x = camPos.x + halfWidth + cameraEdgePadding;
                    float y = Random.Range(camPos.y - halfHeight, camPos.y + halfHeight);
                    candidate = new Vector3(x, y, 0.0f);
                }
            }

            Collider2D hit = Physics2D.OverlapCircle(candidate, overlapCheckRadius, envitonmentMask);
            if(hit == null)
            {
                result = candidate;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }

    public void SetDynamicMaxAlive(int value)
    {
        if (value > 0)
        {
            maxAlive = value;
        }
    }

    public void SetDynamicSpawnsPerTick(int value)
    {
        if (value > 0)
        {
            spawnsPerTick = value;
        }
    }

    public int GetAliveCount()
    {
        int count = 0;

        for (int i = aliveList.Count - 1; i >= 0; i = i - 1)
        {
            if (aliveList[i] == null)
            {
                aliveList.RemoveAt(i);
            }
            else
            {
                count = count + 1;
            }
        }

        return count;
    }
}

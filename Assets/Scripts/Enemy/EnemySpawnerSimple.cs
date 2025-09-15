using UnityEngine;

public class EnemySpawnerSimple : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private float spawnRadius = 8.0f;

    [SerializeField]
    private float intervalSeconds = 2.0f;

    [SerializeField]
    private bool isRunning = true;

    private float accumulatedSeconds = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if(isRunning == true)
        {
            accumulatedSeconds += Time.deltaTime;
            if(accumulatedSeconds >= intervalSeconds)
            {
                TrySpawnOne();
                accumulatedSeconds = 0.0f;
                isRunning = false;
            }
        }
    }

    private void TrySpawnOne()
    {
        if(enemyPrefab != null)
        {
            float angle = Random.Range(0.0f, 360.0f);
            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0.0f) * spawnRadius;
            Vector3 spawnPosition = transform.position + offset;

            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            EnemySimple enemySimple = enemy.GetComponent<EnemySimple>();
            if(enemySimple != null)
            {
                if(target != null)
                {
                    enemySimple.SetTarget(target);
                }
            }
        }
    }

    public void SetRunning(bool newRunning)
    {
        if(newRunning == true)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }
}

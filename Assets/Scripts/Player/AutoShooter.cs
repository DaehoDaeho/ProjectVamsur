using UnityEngine;

public class AutoShooter : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private float fireIntervalSeconds = 0.5f;

    [SerializeField]
    private float projectileDamage = 50.0f;

    [SerializeField]
    private float muzzleOffset = 0.5f;

    [SerializeField]
    private float detectRadius = 10.0f;

    [SerializeField]
    private LayerMask enemyLayerMask;

    private float lastFireTime = -9999.0f;

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastFireTime >= fireIntervalSeconds)
        {
            TryFire();
        }
    }

    void TryFire()
    {
        Transform nearest = FindNearestEnemy();

        if (nearest == null)
        {
            return;
        }

        Vector3 dir3 = nearest.position - transform.position;

        Vector2 dir = new Vector2(dir3.x, dir3.y).normalized;

        Vector3 spawnPos = transform.position + new Vector3(dir.x, dir.y, 0.0f) * muzzleOffset;

        GameObject go = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        ProjectileSimple p = go.GetComponent<ProjectileSimple>();
        if(p != null)
        {
            p.SetDirectionAndDamage(dir, projectileDamage);
        }

        lastFireTime = Time.time;
    }

    Transform FindNearestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectRadius, enemyLayerMask);

        Transform best = null;

        float bestSqr = 0.0f;

        for(int i=0;i<hits.Length; ++i)
        {
            EnemyHealth enemy = hits[i].GetComponent<EnemyHealth>();
            if(enemy != null)
            {
                Vector3 diff = hits[i].transform.position - transform.position;
                float d2 = diff.sqrMagnitude;

                if(best == null)
                {
                    best = hits[i].transform;
                    bestSqr = d2;
                }
                else
                {
                    if(d2 < bestSqr)
                    {
                        best = hits[i].transform;
                        bestSqr = d2;
                    }
                }
            }
        }

        return best;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}

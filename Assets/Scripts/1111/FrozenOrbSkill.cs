using UnityEngine;

public class FrozenOrbSkill : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject frozenOrbPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Skill Settings")]
    [SerializeField] private float cooldown = 3f;
    [SerializeField] private KeyCode skillKey = KeyCode.Q;

    [SerializeField] private SpriteRenderer sr;

    private float cooldownTimer;

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(skillKey) && cooldownTimer <= 0)
        {
            CastFrozenOrb();
            cooldownTimer = cooldown;
        }
    }

    void CastFrozenOrb()
    {
        if (frozenOrbPrefab == null) return;

        // 마우스 방향으로 발사
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 direction = (mousePos - transform.position).normalized;
        Vector2 direction = sr?.flipX == true ? Vector2.left : Vector2.right;

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        GameObject orb = Instantiate(frozenOrbPrefab, spawnPos, Quaternion.identity);

        FrozenOrb orbScript = orb.GetComponent<FrozenOrb>();
        if (orbScript != null)
        {
            orbScript.Initialize(direction);
        }
    }

    // 뱀서라이크에서 자동 발사용
    public void CastAtNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return;

        // 가장 가까운 적 찾기
        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy;
            }
        }

        if (nearest != null && cooldownTimer <= 0)
        {
            Vector2 direction = (nearest.transform.position - transform.position).normalized;

            Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
            GameObject orb = Instantiate(frozenOrbPrefab, spawnPos, Quaternion.identity);

            FrozenOrb orbScript = orb.GetComponent<FrozenOrb>();
            if (orbScript != null)
            {
                orbScript.Initialize(direction);
            }

            cooldownTimer = cooldown;
        }
    }
}
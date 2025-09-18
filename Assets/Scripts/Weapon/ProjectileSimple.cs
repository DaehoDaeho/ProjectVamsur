using UnityEngine;

public class ProjectileSimple : MonoBehaviour
{
    [SerializeField]
    private float speed = 10.0f;

    [SerializeField]
    private float lifeSeconds = 3.0f;

    [SerializeField]
    private float damage = 50.0f;

    private Vector2 moveDirection = Vector2.right;
    private float spawnTime = 0.0f;

    private void OnEnable()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 delta = new Vector3(moveDirection.x, moveDirection.y, 0.0f) * speed * Time.deltaTime;

        transform.position += delta;

        if(Time.time - spawnTime >= lifeSeconds)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirectionAndDamage(Vector2 dir, float dmg)
    {
        moveDirection = dir.normalized;
        damage = dmg;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") == true)
        {
            EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}

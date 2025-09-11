using UnityEngine;

public class EnemySimple : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float movespeed = 1.5f;

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            Vector3 direction = target.position - transform.position;

            if(direction.sqrMagnitude > 0.0001f)
            {
                direction = direction.normalized;
                transform.position = transform.position + (direction * movespeed * Time.deltaTime);
            }
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}

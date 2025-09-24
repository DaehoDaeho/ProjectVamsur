using UnityEngine;

public class PooledObject : MonoBehaviour
{
    private PrefabPool ownerPool;

    public void SetOwnerPool(PrefabPool pool)
    {
        ownerPool = pool;
    }

    public void ReturnToPool()
    {
        if (ownerPool != null)
        {
            ownerPool.Return(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

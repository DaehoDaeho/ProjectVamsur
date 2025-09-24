using UnityEngine;
using System.Collections.Generic;

public class PrefabPool : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private int prewarmCount = 30;

    [SerializeField]
    private bool allowExpand = true;

    private readonly Queue<GameObject> poolQueue = new Queue<GameObject>();

    private void Awake()
    {
        for(int i=0; i<prewarmCount; ++i)
        {
            GameObject go = CreateInstance();
            Return(go);
        }
    }

    GameObject CreateInstance()
    {
        GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);

        PooledObject marker = go.GetComponent<PooledObject>();
        if(marker == null)
        {
            marker = go.AddComponent<PooledObject>();
        }

        marker.SetOwnerPool(this);

        return go;
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject go = null;

        if(poolQueue.Count > 0)
        {
            go = poolQueue.Dequeue();
        }
        else
        {
            if(allowExpand == true)
            {
                go = CreateInstance();
            }
            else
            {
                return null;
            }
        }

        if(go != null)
        {
            go.transform.SetPositionAndRotation(position, rotation);
            go.SetActive(true);
        }

        return go;
    }

    public void Return(GameObject go)
    {
        go.SetActive(false);
        go.transform.SetParent(transform);
        poolQueue.Enqueue(go);
    }
}

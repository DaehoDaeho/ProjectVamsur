using UnityEngine;

public class DropOnDeathXP : MonoBehaviour
{
    [SerializeField]
    private GameObject xpOrbPrefab;

    [SerializeField]
    private int orbCount = 3;

    [SerializeField]
    private  int xpPerOrb = 1;

    [SerializeField]
    private float scatterRadius = 0.5f;

    public void SpawnOrb()
    {
        for(int i=0; i<orbCount; ++i)
        {
            float angleDeg = (360.0f / orbCount) * i;
            float rad = angleDeg * Mathf.Deg2Rad;

            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

            float scale = Random.Range(0.3f, 1.0f);
            Vector3 pos = transform.position + new Vector3(dir.x, dir.y, 0.0f) * scatterRadius * scale;

            GameObject go = Instantiate(xpOrbPrefab, pos, Quaternion.identity);
            if(go != null)
            {
                XPOrb orb = go.GetComponent<XPOrb>();
                if(orb != null)
                {
                    orb.SetValue(xpPerOrb);
                }
            }
        }
    }
}

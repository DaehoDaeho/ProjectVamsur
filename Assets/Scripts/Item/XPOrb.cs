using UnityEngine;

public class XPOrb : MonoBehaviour
{
    [SerializeField]
    private int xpValue = 1;

    [SerializeField]
    private float pickupRadius = 0.6f;

    [SerializeField]
    private float magnetRadius = 3.0f;

    [SerializeField]
    private float moveSpeedBase = 3.0f;

    [SerializeField]
    private float moveSpeedBonusByDistance = 2.0f;

    private Transform player;
    private bool isMagnetized = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if(p != null)
        {
            player = p.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            return;
        }

        Vector3 toPlayer = player.position - transform.position;
        float d = toPlayer.magnitude;

        if (d <= pickupRadius)
        {
            AbsorbIntoPlayer();
            return;
        }

        if(isMagnetized == false)
        {
            if(d <= magnetRadius)
            {
                isMagnetized = true;
            }
        }

        if(isMagnetized == true)
        {
            Vector3 dir = toPlayer.normalized;
            float speed = moveSpeedBase + d * moveSpeedBonusByDistance;

            Vector3 delta = dir * speed * Time.deltaTime;
            transform.position += delta;
        }
    }

    public void SetValue(int v)
    {
        xpValue = v;
    }

    private void AbsorbIntoPlayer()
    {
        PlayerExperience exp = player.GetComponent<PlayerExperience>();

        if(exp != null)
        {
            exp.AddXP(xpValue);
        }

        Destroy(gameObject);
    }
}

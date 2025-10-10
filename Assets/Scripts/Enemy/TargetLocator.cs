using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    public Transform playerTransform;

    private void Awake()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if(p != null)
        {
            playerTransform = p.transform;
        }
    }
}

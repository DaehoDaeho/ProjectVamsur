using UnityEngine;

public class LifeCycleLogger : MonoBehaviour
{
    GameObject character;
    float moveSpeed = 5.0f;

    // Update is called once per frame
    void Start()
    {
        int a = 10;
        int b = 20;

        // reference.
        Function(ref a, ref b);

        Debug.Log("a = " + a);
        Debug.Log("b = " + b);
    }

    public void Function(ref int a, ref int b)
    {
        a = 30;
        b = 40;
    }
}

using UnityEngine;

public class LifeCycleLogger : MonoBehaviour
{
    float timer = 0.0f;
    float fixedTimer = 0.0f;       

    // Update is called once per frame
    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        transform.position += new Vector3(moveInput * Time.deltaTime, 0.0f, 0.0f);
    }
}

using UnityEngine;

public class AnimatorVelocity : MonoBehaviour
{
    [SerializeField]
    private string speedParameterName = "Speed";

    [SerializeField]
    private SpriteRenderer sr;

    [SerializeField]
    private float smoothing = 10.0f;

    [SerializeField]
    private Animator animator;

    private Vector3 lastPosition;
    private float currentSpeed = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 delta = transform.position - lastPosition;
        float rawSpeed = delta.magnitude / Time.deltaTime;

        currentSpeed = Mathf.Lerp(currentSpeed, rawSpeed, smoothing * Time.deltaTime);

        animator.SetFloat(speedParameterName, currentSpeed);

        if(/*Mathf.Abs(delta.x) > Mathf.Abs(delta.y) &&*/ Mathf.Abs(delta.x) > 0.001f)
        {
            if(delta.x < 0.0f)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }
        }

        lastPosition = transform.position;
    }
}

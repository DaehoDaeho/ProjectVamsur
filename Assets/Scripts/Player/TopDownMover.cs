using UnityEngine;

public class TopDownMover : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed = 5.0f;

    [SerializeField]
    private float acceleration = 20.0f;

    [SerializeField]
    private float deceleration = 25.0f;

    [SerializeField]
    private Rigidbody2D rigidbody2D;

    [SerializeField]
    private MapBounds mapBounds;

    private Vector2 targetVelocity = Vector2.zero;
    private Vector2 currentVelocity = Vector2.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = Vector2.zero;
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        targetVelocity = moveInput.normalized * maxSpeed;
    }

    private void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime;

        // ����/�������� currentVelocity�� targetVelocity�� ������Ų��.
        Vector2 toTarget = targetVelocity - currentVelocity;

        float accel = 0.0f;

        if(targetVelocity.sqrMagnitude > 0.0f)
        {
            accel = acceleration;
        }
        else
        {
            accel = deceleration;
        }

        Vector2 step = Vector2.ClampMagnitude(toTarget, accel * deltaTime);
        currentVelocity = currentVelocity + step;

        Vector2 newPosition = rigidbody2D.position + currentVelocity * deltaTime;

        float minX;
        float maxX;
        float minY;
        float maxY;

        // ������ ���� - ������ ������ �� ������ �޸� �ּҸ� �����Ѵ�.
        // ���� �ٸ� �Լ����� ������ ���� �����ϸ� ���� ������ ���� ����ȴ�.
        mapBounds.GetWorldBounds(out minX, out maxX, out minY, out maxY);

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        rigidbody2D.MovePosition(newPosition);
    }
}

using UnityEngine;

public class DashAbility : MonoBehaviour
{
    [SerializeField]
    private float dashSpeed = 15.0f;

    [SerializeField]
    private float dashDurationSeconds = 0.2f;

    [SerializeField]
    private float dashCooldownSeconds = 1.0f;

    [SerializeField]
    private float invulnerabilitySeconds = 0.5f;

    [SerializeField]
    private TopDownMover topDownMover;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private InvulnerabilityWindow invulnerabilityWindow;

    [SerializeField]
    private Rigidbody2D rigidbody2D;

    private bool isDashing = false;
    private float dashEndTime = 0.0f;
    private float nextAvailableDashTime = 0.0f;

    private Vector2 lastMoveDir = Vector2.right;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isDashing == false)
        {
            Vector2 move = Vector2.zero;
            move.x = Input.GetAxis("Horizontal");
            move.y = Input.GetAxis("Vertical");

            if(move.sqrMagnitude > 0.0001f)
            {
                lastMoveDir = move.normalized;
            }
            else
            {
                if(spriteRenderer.flipX == true)
                {
                    lastMoveDir = Vector2.left;
                }
                else
                {
                    lastMoveDir = Vector2.right;
                }
            }

            if(Input.GetKeyDown(KeyCode.Space) == true)
            {
                if(Time.time >= nextAvailableDashTime)
                {
                    StartDash();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if(isDashing == true)
        {
            float delta = Time.fixedDeltaTime;
            Vector2 newPos = rigidbody2D.position + lastMoveDir * dashSpeed * delta;

            rigidbody2D.MovePosition(newPos);

            if(Time.time >= dashEndTime)
            {
                EndDash();
            }
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashEndTime = Time.time + dashDurationSeconds;
        nextAvailableDashTime = Time.time + dashCooldownSeconds;

        topDownMover.enabled = false;
        invulnerabilityWindow.GrantInvulnerability(invulnerabilitySeconds);
    }

    void EndDash()
    {
        isDashing = false;

        topDownMover.enabled = true;
    }

    public bool GetIsDashing()
    {
        return isDashing;
    }
}

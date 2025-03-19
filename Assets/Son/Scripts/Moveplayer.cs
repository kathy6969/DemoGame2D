using System.Collections;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    public float dashForce = 10f;
    public float dashTime = 0.3f;
    public float dashCooldown = 1f;  // Thêm thời gian hồi phục dash
    public Transform groundCheck;
    public LayerMask groundLayer; // LayerMask để kiểm tra đất
    private PlayerRotation playerRotation; // Tham chiếu đến script PlayerRotation

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private float groundCheckRadius = 0.2f;

    private bool canDash;
    private bool isDashing = false;
    private float lastDashTime = 0f;  // Lưu thời gian của lần dash cuối

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerRotation = GetComponent<PlayerRotation>(); // Lấy tham chiếu đến PlayerRotation
    }

    void Update()
    {
        // Kiểm tra nếu slider đầy và đã đủ thời gian hồi phục
        canDash = DashManager.Instance.dashSlider.value == 1 && Time.time >= lastDashTime + dashCooldown;

        if (!isDashing)
        {
            HandleMovement();
        }

        HandleJump();
        HandleDash();
        CheckGround();
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");

        animator.SetBool("isMoving", moveInput != 0);
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Quay player theo hướng di chuyển
        if (PlayerShooting.Shot == false)
        {
            if (moveInput > 0 && !playerRotation.facingRight)
            {
                playerRotation.Flip();
            }
            else if (moveInput < 0 && playerRotation.facingRight)
            {
                playerRotation.Flip();
            }
        }
    }

    private void HandleJump()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
        }
    }

    private void HandleDash()
    {
        // Kiểm tra khi người chơi nhấn Space và có thể Dash
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        rb.velocity = Vector2.zero;  // Dừng lại hoàn toàn trước khi dash
        float dashDirection = playerRotation.facingRight ? 1f : -1f;
        rb.AddForce(new Vector2(dashDirection * dashForce, 0), ForceMode2D.Impulse);

        // Lưu lại thời gian của lần dash
        lastDashTime = Time.time;

        // Tạm dừng trong một khoảng thời gian dash
        yield return new WaitForSeconds(dashTime);
        isDashing = false;

        // Reset slider về 0 sau khi dash xong và bắt đầu lại tiến trình nạp đầy slider
        DashManager.Instance.ResetDash();
    }

    private void CheckGround()
    {
        // Kiểm tra va chạm với LayerMask "groundLayer"
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("isJumping", !isGrounded);
    }
}

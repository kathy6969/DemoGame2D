using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpForce = 7f;
    public float dashForce = 10f;
    public float dashTime = 0.3f;
    public float dashCooldown = 1f;  // Thời gian hồi phục dash

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer; // LayerMask để kiểm tra đất

    [Header("References")]
    public Animator animator; // Animator của player

    [Header("Rotation Settings")]
    public bool facingRight = true; // Kiểm tra hướng quay của player

    [Header("Canvas Reference")]
    public Canvas playerCanvas; // Tham chiếu đến Canvas con của player

    // Các biến nội bộ
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool canDash;
    private bool isDashing = false;
    private float lastDashTime = 0f;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Kiểm tra nếu slider dash đầy và đủ cooldown (ví dụ sử dụng DashManager)
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

        if (PlayerShooting.Shot == false)
        {
            if (moveInput > 0)
            {
                Flip();
                spriteRenderer.flipX = false;
            }
            else if (moveInput < 0)
            {
                Flip();
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            if (mousePos.x > transform.position.x)
            {
                Flip();
                spriteRenderer.flipX = false; // Hướng phải
            }
            else if (mousePos.x < transform.position.x)
            {
                Flip();
                spriteRenderer.flipX = true; // Hướng trái
            }
        }
    }

    private void HandleJump()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            // Reset tốc độ y và nhảy
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
        }
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        rb.velocity = Vector2.zero;  // Dừng lại trước khi dash
        float dashDirection = 0;
        if (spriteRenderer.flipX==false)
        {
            dashDirection = 1f;
        }
        else
        {
            dashDirection = -1f;
        }
        rb.AddForce(new Vector2( dashDirection * dashForce,0), ForceMode2D.Impulse);
        lastDashTime = Time.time;

        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        DashManager.Instance.ResetDash();
    }

    private void CheckGround()
    {
        // Kiểm tra va chạm với đất
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("isJumping", !isGrounded);
    }
    public void Flip()
    {
        facingRight = !facingRight;
    }
  
}

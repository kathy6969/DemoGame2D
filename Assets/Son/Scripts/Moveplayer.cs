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

    // Lưu ý: DashManager và PlayerShooting là các script/thành phần riêng.
    // Ví dụ: DashManager.Instance.dashSlider.value và PlayerShooting.Shot
    // Nếu chưa có, bạn cần đảm bảo thêm hoặc điều chỉnh theo dự án của mình.

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

        // Nếu không đang bắn (PlayerShooting.Shot false), cập nhật hướng quay
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

        // Dựa vào hướng hiện tại của player để xác định vector dash
        //float dashDirection = facingRight ? 1f : 0f;
        //rb.AddForce(new Vector2(dashDirection * dashForce, 0), ForceMode2D.Impulse);
        if (facingRight)
        {
            dashDirection = 1f;
        }
        else
        {
            dashDirection = -1f;
        }
        rb.AddForce(new Vector2( dashDirection * dashForce,0), ForceMode2D.Impulse);

        // Cập nhật thời gian dash
        lastDashTime = Time.time;

        yield return new WaitForSeconds(dashTime);
        isDashing = false;

        // Reset slider dash qua DashManager (nếu có)
        DashManager.Instance.ResetDash();
    }

    private void CheckGround()
    {
        // Kiểm tra va chạm với đất
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("isJumping", !isGrounded);
    }

    /// <summary>
    /// Flip sẽ đảo ngược hướng của player. Sau khi flip, reset scale của Canvas về Vector3.one
    /// để UI không bị lật theo.
    /// </summary>
    public void Flip()
    {
        facingRight = !facingRight;
        //Vector3 scale = transform.localScale;
        //scale.x *= -1;
        //transform.localScale = scale;

        //// Nếu có gán playerCanvas, đặt lại localScale cho Canvas
        //if (playerCanvas != null)
        //{
        //    playerCanvas.transform.localScale = Vector3.one;
        //}
    }

    /// <summary>
    /// Hàm RotateToDirection nếu cần chuyển hướng riêng cho các mục đích khác.
    /// </summary>
    //public void RotateToDirection(Vector2 direction)
    //{
    //    if (direction.x > 0 && !facingRight)
    //    {
    //        Flip();
    //    }
    //    else if (direction.x < 0 && facingRight)
    //    {
    //        Flip();
    //    }
    //}
}

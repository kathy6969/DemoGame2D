using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;         // Tốc độ chạy
    public float jumpForce = 7f;     // Lực nhảy
    private Rigidbody2D rb;          // RigidBody của nhân vật
    private Animator animator;       // Animator để điều khiển animation
    private bool isGrounded = true;  // Kiểm tra xem nhân vật có đang trên mặt đất không
    private bool facingRight = true; // Kiểm tra hướng nhân vật

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal"); // Lấy input từ bàn phím (A, D, ←, →)

        // Kiểm tra có đang di chuyển không
        animator.SetBool("isMoving", moveInput != 0);

        // Di chuyển nhân vật
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Lật mặt nhân vật theo hướng di chuyển
        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); // Reset vận tốc Y trước khi nhảy
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
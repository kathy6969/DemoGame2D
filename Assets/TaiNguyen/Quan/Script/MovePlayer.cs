using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float speed = 5f;         // Tốc độ chạy
    public float jumpForce = 7f;     // Lực nhảy
    public Transform groundCheck;    // Vị trí kiểm tra mặt đất
    public LayerMask groundLayer;    // Layer của mặt đất
    private Rigidbody2D rb;          // RigidBody của nhân vật
    private Animator animator;       // Animator để điều khiển animation
    private bool isGrounded;         // Kiểm tra xem nhân vật có đang trên mặt đất không
    private bool facingRight = true; // Kiểm tra hướng nhân vật
    private float groundCheckRadius = 0.2f; // Bán kính kiểm tra mặt đất

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        CheckGround();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal"); // Lấy input từ bàn phím (A, D, ←, →)

        // Kiểm tra có đang di chuyển không
    

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
         
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
  
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

  
}

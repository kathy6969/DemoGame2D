using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    public float dashForce = 10f;  
    public float dashTime = 0.3f;    
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private bool facingRight = true;
    private float groundCheckRadius = 0.2f;

    private bool canDash;
    private bool isDashing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (DashManager.Instance.dashSlider.value == 1)
        {
            canDash = true;
        }
        else
        {
            canDash = false;
        }
        if (!isDashing)
        {
            HandleMovement();
        }
        HandleJump();
        HandleDash();
        CheckGround();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");

        animator.SetBool("isMoving", moveInput != 0);
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

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
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) // Nhấn W hoặc ↑ để nhảy
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                animator.SetBool("isJumping", true);
            }
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
        DashManager.Instance.ResetDash();
        isDashing = true;
        float dashDirection = facingRight ? 1f : -1f;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(dashDirection * dashForce, 0), ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        Debug.Log("isGrounded = " + isGrounded);
        animator.SetBool("isJumping", !isGrounded);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Dame"))
        {
            Destroy(gameObject);
        }
    }
}

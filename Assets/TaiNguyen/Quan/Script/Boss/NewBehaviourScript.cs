using UnityEngine;
using System.Collections;

public class BossAI_WithAttackAndMovement : MonoBehaviour
{
    [Header("References")]
    public Transform player;                   // Transform của player
    public Animator animator;                  // Animator của boss
    public Rigidbody2D rb;                     // Rigidbody2D của boss
    public SpriteRenderer spriteRenderer;      // SpriteRenderer để hiển thị và flip sprite

    [Header("Movement Settings")]
    public float moveSpeed = 2f;               // Tốc độ di chuyển khi không phát hiện player
    public Transform groundCheck;              // Vị trí kiểm tra ground dưới chân
    public float groundCheckRadius = 0.2f;       // Bán kính kiểm tra ground
    public Transform siteCheck;                // Vị trí kiểm tra phía trước
    public float siteCheckRadius = 0.2f;         // Bán kính kiểm tra site
    public LayerMask groundLayer;              // Layer của các đối tượng ground
    public float flipCooldownDuration = 1f;    // Thời gian chờ sau khi quay đầu

    [Header("Detection & Attack Settings")]
    public float detectionRange = 10f;         // Phạm vi phát hiện player
    public float attackRange = 2f;             // Khoảng cách đủ để tấn công
    public float chaseSpeed = 3f;              // Tốc độ đuổi theo player
    public float attackCooldown = 5f;          // Thời gian chờ sau mỗi đợt tấn công

    [Header("Ultimate Attack Settings")]
    public float ultimateAttackDuration = 3f;  // Thời gian thi triển của chiêu tối thượng (Attack 4)

    [Header("Teleport Settings")]
    public float teleportCooldown = 5f;        // Thời gian chờ giữa các lần teleport

    // Nội bộ
    private bool isAttacking = false;          // Cờ báo hiệu boss đang tấn công
    private int lastAttack = -1;               // Kiểu attack vừa thực hiện
    private int repeatCount = 0;               // Số lần lặp lại cùng kiểu attack
    private bool facingRight = true;           // Theo dõi hướng của boss
    private float lastFlipTime = -100f;        // Thời gian quay đầu gần nhất
    private float lastTeleportTime = -100f;    // Thời gian teleport gần nhất

    void Start()
    {
        // Đặt weight của Attack Layer (layer 1) mặc định = 0
        animator.SetLayerWeight(1, 0);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        bool playerDetected = (distance <= detectionRange);

        // Nếu boss đang tấn công, không cho di chuyển
        if (isAttacking)
            return;

        if (playerDetected)
        {
            // Kiểm tra teleport nếu đủ cooldown
            if (Time.time >= lastTeleportTime + teleportCooldown)
            {
                // Kiểm tra xem người chơi có ở phía trước không
                bool isPlayerInFront = ((player.position.x - transform.position.x > 0) && facingRight) ||
                                       ((player.position.x - transform.position.x < 0) && !facingRight);
                if (!isPlayerInFront)
                {
                    // Nếu người chơi phía sau, quay đầu (flip) và không teleport ngay
                    Flip();
                    lastTeleportTime = Time.time; // Cập nhật cooldown để tránh flip liên tục
                    return;
                }
                TeleportToPlayer();
                lastTeleportTime = Time.time;
                return;
            }

            // Nếu không đủ cooldown teleport, tiếp tục chế độ chase hoặc attack
            if (distance > attackRange)
            {
                ChasePlayer();
            }
            else
            {
                rb.velocity = Vector2.zero;
                animator.SetBool("isRunning", false);
                animator.SetLayerWeight(1, 1);
                StartCoroutine(HandleAttack());
            }
        }
        else
        {
            // Nếu không phát hiện player, thực hiện di chuyển và kiểm tra môi trường
            Move();
        }
    }

    // Phương thức di chuyển mặc định: đi về phía trước
    void Move()
    {
        // Kiểm tra ground dưới chân và chướng ngại phía trước
        bool isGroundAhead = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        bool isSiteBlocked = Physics2D.OverlapCircle(siteCheck.position, siteCheckRadius, groundLayer);

        // Nếu không có ground hoặc phía trước có chướng ngại, dừng lại và quay đầu (nếu đủ thời gian)
        if (!isGroundAhead || isSiteBlocked)
        {
            rb.velocity = Vector2.zero;
            if (Time.time >= lastFlipTime + flipCooldownDuration)
            {
                Flip();
                lastFlipTime = Time.time;
            }
            return;
        }

        // Di chuyển theo hướng mặc định (theo biến facingRight)
        float moveDir = facingRight ? 1f : -1f;
        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);
        animator.SetBool("isRunning", true);
    }

    // Chế độ Chase: boss đuổi theo player
    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y);
        animator.SetBool("isRunning", true);
        Flip(direction.x);
    }

    // Teleport boss tới vị trí của người chơi (giữ nguyên trục Z)
    void TeleportToPlayer()
    {
        Vector3 targetPos = player.position;
        targetPos.z = transform.position.z;
        transform.position = targetPos;
        Debug.Log("Teleported to player at " + targetPos);
    }

    // Coroutine xử lý tấn công
    IEnumerator HandleAttack()
    {
        isAttacking = true;
        int attackType = ChooseAttackType();
        animator.SetInteger("attackType", attackType);
        animator.SetBool("canAttack", true);

        switch (attackType)
        {
            case 1:
            case 2:
                yield return StartCoroutine(Attack_Run());
                break;
            case 3:
                yield return StartCoroutine(Attack_Dash());
                break;
            case 4:
                yield return StartCoroutine(Attack_Ultimate());
                break;
        }

        animator.SetBool("canAttack", false);
        animator.SetLayerWeight(1, 0);
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    // Hàm chọn kiểu tấn công ngẫu nhiên (1-4) với giới hạn không lặp lại quá 3 lần cùng kiểu
    int ChooseAttackType()
    {
        int attackType;
        do
        {
            attackType = Random.Range(1, 5); // Chọn số từ 1 đến 4
        } while (attackType == lastAttack && repeatCount >= 3);

        if (attackType == lastAttack)
            repeatCount++;
        else
            repeatCount = 0;

        lastAttack = attackType;
        return attackType;
    }

    // Kiểu tấn công: Chạy tấn công (Attack 1 & 2)
    IEnumerator Attack_Run()
    {
        Debug.Log("Executing Run Attack");
        float attackSpeed = 5f;
        float attackDuration = 1f;
        float timer = 0f;
        Vector2 direction = (player.position - transform.position).normalized;
        Flip(direction.x);
        while (timer < attackDuration)
        {
            rb.velocity = new Vector2(direction.x * attackSpeed, rb.velocity.y);
            timer += Time.deltaTime;
            yield return null;
        }
        rb.velocity = Vector2.zero;
    }

    // Kiểu tấn công: Lướt tấn công (Attack 3)
    IEnumerator Attack_Dash()
    {
        Debug.Log("Executing Dash Attack");
        Vector2 startPos = transform.position;
        Vector2 targetPos = player.position;
        float dashDuration = 0.5f;
        float timer = 0f;
        Flip(player.position.x - transform.position.x);
        while (timer < dashDuration)
        {
            transform.position = Vector2.Lerp(startPos, targetPos, timer / dashDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
        yield return new WaitForSeconds(0.5f);
    }

    // Kiểu tấn công: Ultimate Attack (Attack 4) với thời gian thi triển dài hơn
    IEnumerator Attack_Ultimate()
    {
        Debug.Log("Executing Ultimate Attack (Attack 4)");
        yield return new WaitForSeconds(ultimateAttackDuration);
    }

    // Hàm Flip: với tham số hướng nếu cần thiết
    void Flip(float moveDirection)
    {
        if (moveDirection > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveDirection < 0 && facingRight)
        {
            Flip();
        }
    }

    // Overload Flip(): đảo hướng hiện tại
    void Flip()
    {
        facingRight = !facingRight;
        // Lật sprite bằng cách đảo scale.x
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Vẽ Gizmos để hiển thị vùng kiểm tra trong Scene view
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        if (siteCheck != null)
            Gizmos.DrawWireSphere(siteCheck.position, siteCheckRadius);

        if (player != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
}

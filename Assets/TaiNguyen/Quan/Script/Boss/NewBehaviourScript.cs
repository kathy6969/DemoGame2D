using UnityEngine;
using System.Collections;

public class BossAI_WithAttackAndMovement : MonoBehaviour
{
    [Header("References")]
    public Transform player;                   // Transform của player
    public Animator animator;                  // Animator của boss (sử dụng layer để chuyển đổi trạng thái)
    public Rigidbody2D rb;                     // Rigidbody2D của boss
    public SpriteRenderer spriteRenderer;      // SpriteRenderer để hiển thị và flip sprite

    [Header("Movement Settings")]
    public float moveSpeed = 2f;               // Tốc độ di chuyển khi không phát hiện player
    public Transform groundCheck;              // Vị trí kiểm tra ground của boss
    public float groundCheckRadius = 0.2f;
    public Transform siteCheck;                // Vị trí kiểm tra phía trước của boss
    public float siteCheckRadius = 0.2f;         // (Sẽ được nhân đôi)
    public LayerMask groundLayer;
    public float flipCooldownDuration = 1f;

    [Header("Detection & Attack Settings")]
    public float detectionRange = 20f;         // Vùng phát hiện player (20f)
    public float attackRange = 4f;             // Khoảng cách đủ để tấn công (4 đơn vị)
    public float chaseSpeed = 3f;              // Tốc độ đuổi theo (vẫn là tốc độ đi bộ)
    public float attackCooldown = 7f;

    [Header("Ultimate Attack Settings")]
    public float ultimateAttackDuration = 5f;  // Tổng thời gian mỗi kiểu attack là 5 giây

    [Header("Teleport Settings")]
    public float teleportCooldown = 7f;
    [Tooltip("Khoảng cách tối thiểu để thực hiện teleport (ví dụ: 4f)")]
    public float teleportMinDistance = 4f;
    [Tooltip("Khoảng cách tối đa để thực hiện teleport (ví dụ: 20f)")]
    public float teleportMaxDistance = 20f;

    // Nội bộ
    private bool isAttacking = false;
    private bool facingRight = true;
    private float lastFlipTime = -100f;
    private float lastTeleportTime = -100f;

    // Tốc độ tấn công riêng cho các kiểu di chuyển (Attack 1 và 3)
    public float attackMoveSpeed = 3f;       // tốc độ tấn công (boss chạy)

    void Start()
    {
        // Ẩn layer attack ban đầu (giả sử layer attack là layer 1)
        animator.SetLayerWeight(1, 0);
        siteCheckRadius *= 2f; // nhân đôi bán kính của siteCheck
    }

    void Update()
    {
        if (player == null)
            return;

        // Luôn luôn quay mặt về phía player
        float diff = player.position.x - transform.position.x;
        if (diff > 0 && !facingRight)
            Flip();
        else if (diff < 0 && facingRight)
            Flip();

        float distance = Vector2.Distance(transform.position, player.position);
        bool playerDetected = (distance <= detectionRange);

        // Nếu boss đang attack thì chỉ xử lý các logic phụ (ví dụ: dừng trạng thái chạy nếu có chướng ngại)
        if (isAttacking)
        {
            if (Physics2D.OverlapCircle(siteCheck.position, siteCheckRadius, groundLayer))
                animator.SetBool("isRunning", false);
            return;
        }

        if (playerDetected)
        {
            // Kiểm tra teleport: nếu đủ cooldown và khoảng cách nằm trong giới hạn cho phép
            if (Time.time >= lastTeleportTime + teleportCooldown &&
                distance >= teleportMinDistance && distance <= teleportMaxDistance)
            {
                TeleportToPlayer();
                lastTeleportTime = Time.time;
                return;
            }

            // Nếu player ở trong phạm vi tấn công (4 đơn vị), boss dừng di chuyển và tấn công
            if (distance <= attackRange)
            {
                rb.velocity = Vector2.zero;
                animator.SetBool("isRunning", false);
                animator.SetLayerWeight(1, 1);
                StartCoroutine(HandleAttack());
            }
            else
            {
                // Nếu player ngoài phạm vi tấn công, boss sẽ đuổi theo player
                ChasePlayer();
            }
        }
        else
        {
            // Nếu không có player, boss di chuyển theo chế độ patrol/dựa vào groundCheck và siteCheck
            Move();
        }
    }

    void Move()
    {
        bool isGroundAhead = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        bool isSiteBlocked = Physics2D.OverlapCircle(siteCheck.position, siteCheckRadius, groundLayer);
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

        float moveDir = facingRight ? 1f : -1f;
        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);
        animator.SetBool("isRunning", true);
    }

    void ChasePlayer()
    {
        // Boss đuổi theo player
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y);
        animator.SetBool("isRunning", true);
        Flip(direction.x);
    }

    void TeleportToPlayer()
    {
        Vector3 targetPos = player.position;
        targetPos.z = transform.position.z;
        transform.position = targetPos;
        Debug.Log("Teleported to player at " + targetPos);
    }

    IEnumerator HandleAttack()
    {
        isAttacking = true;
        int attackType = ChooseAttackType();
        animator.SetInteger("attackType", attackType);
        animator.SetBool("canAttack", true);

        // Giữ nguyên logic tấn công hiện tại
        switch (attackType)
        {
            case 1:
                yield return StartCoroutine(Attack_Run());
                break;
            case 2:
                yield return StartCoroutine(Attack_FireOrbs());
                break;
            case 3:
                yield return StartCoroutine(Attack_Dash());
                break;
            case 4:
                yield return StartCoroutine(Attack_FireOrbStraight());
                break;
            case 5:
                yield return StartCoroutine(Attack_SummonSkeletons());
                break;
            case 6:
                yield return StartCoroutine(Attack_FireballsFromAbove());
                break;
            case 7:
                yield return StartCoroutine(Attack_ShootFireball());
                break;
        }

        animator.SetBool("canAttack", false);
        animator.SetLayerWeight(1, 0);
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    // Chọn kiểu tấn công ngẫu nhiên từ 1 đến 7
    int ChooseAttackType()
    {
        return Random.Range(1, 8);
    }

    // --- Các kiểu Attack ---
    IEnumerator Attack_Run()
    {
        Debug.Log("Executing Run Attack (Attack 1)");
        float duration = 1f;
        float timer = 0f;
        Vector2 direction = (player.position - transform.position).normalized;
        while (timer < duration)
        {
            rb.velocity = new Vector2(direction.x * attackMoveSpeed, rb.velocity.y);
            timer += Time.deltaTime;
            yield return null;
        }
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(5f - duration);
    }

    IEnumerator Attack_Dash()
    {
        Debug.Log("Executing Dash Attack (Attack 3)");
        Vector2 startPos = transform.position;
        float dashDistance = 4f;
        Vector2 targetPos = startPos + new Vector2(facingRight ? dashDistance : -dashDistance, 0);
        float duration = 0.5f;
        float timer = 0f;
        while (timer < duration)
        {
            transform.position = Vector2.Lerp(startPos, targetPos, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
        yield return new WaitForSeconds(5f - duration);
    }

    IEnumerator Attack_FireOrbs()
    {
        Debug.Log("Executing Fire Orbs Attack (Attack 2)");
        yield return new WaitForSeconds(5f);
    }

    IEnumerator Attack_FireOrbStraight()
    {
        Debug.Log("Executing Fire Orb Straight Attack (Attack 4)");
        yield return new WaitForSeconds(5f);
    }

    IEnumerator Attack_SummonSkeletons()
    {
        Debug.Log("Executing Summon Skeletons Attack (Attack 5)");
        yield return new WaitForSeconds(5f);
    }

    IEnumerator Attack_FireballsFromAbove()
    {
        Debug.Log("Executing Fireballs From Above Attack (Attack 6)");
        yield return new WaitForSeconds(5f);
    }

    IEnumerator Attack_ShootFireball()
    {
        Debug.Log("Executing Shoot Fireball Attack (Attack 7)");
        yield return new WaitForSeconds(5f);
    }

    // --- Các hàm hỗ trợ ---
    void Flip(float moveDirection)
    {
        if (moveDirection > 0 && !facingRight)
            Flip();
        else if (moveDirection < 0 && facingRight)
            Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

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

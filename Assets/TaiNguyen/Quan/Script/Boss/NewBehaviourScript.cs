using UnityEngine;
using System.Collections;

public class BossAI_WithAttackAndPatrol : MonoBehaviour
{
    [Header("References")]
    public Transform player;         // Transform của player
    public Animator animator;        // Animator của boss
    public Rigidbody2D rb;           // Rigidbody2D của boss
    public SpriteRenderer spriteRenderer; // SpriteRenderer để hiển thị và flip sprite

    [Header("Patrol Settings")]
    public Transform leftPoint;      // Điểm bên trái để patrol
    public Transform rightPoint;     // Điểm bên phải để patrol
    public float patrolSpeed = 2f;   // Tốc độ di chuyển khi patrol
    public float waitTime = 1f;      // Thời gian dừng lại khi đến điểm patrol

    [Header("Detection & Attack Settings")]
    public float detectionRange = 10f;   // Phạm vi phát hiện player
    public float attackRange = 2f;       // Khoảng cách đủ để tấn công
    public float chaseSpeed = 3f;        // Tốc độ đuổi theo player
    public float attackCooldown = 5f;    // Thời gian chờ sau mỗi đợt tấn công

    [Header("Ultimate Attack Settings")]
    public float ultimateAttackDuration = 3f; // Thời gian thi triển của chiêu tối thượng (Attack 4)

    // Nội bộ
    private Transform currentPatrolTarget; // Điểm patrol hiện tại
    private bool isAttacking = false;        // Cờ báo hiệu boss đang tấn công
    private int lastAttack = -1;             // Kiểu attack vừa thực hiện
    private int repeatCount = 0;             // Số lần lặp lại cùng kiểu attack
    private bool facingRight = true;         // Theo dõi hướng của boss
    private bool isPatrolActive = true;      // Cờ báo hiệu chế độ patrol đang hoạt động

    void Start()
    {
        // Khởi tạo điểm patrol ban đầu: giả sử boss bắt đầu hướng về bên phải
        currentPatrolTarget = rightPoint;
        // Hiện điểm patrol hiện tại và ẩn điểm kia
        rightPoint.gameObject.SetActive(true);
        leftPoint.gameObject.SetActive(false);
        // Đặt weight của Attack Layer (layer 1) mặc định = 0
        animator.SetLayerWeight(1, 0);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        bool playerDetected = (distance <= detectionRange);

        // Nếu boss đang tấn công, dừng các hoạt động khác
        if (isAttacking)
            return;

        if (playerDetected)
        {
            // Khi phát hiện player, tắt chế độ patrol
            isPatrolActive = false;
            if (distance > attackRange)
            {
                ChasePlayer();
            }
            else
            {
                // Đủ gần để attack: dừng di chuyển, bật Attack Layer, và thực hiện tấn công
                rb.velocity = Vector2.zero;
                animator.SetBool("isRunning", false);
                animator.SetLayerWeight(1, 1);
                StartCoroutine(HandleAttack());
            }
        }
        else
        {
            // Không phát hiện player: bật chế độ patrol
            isPatrolActive = true;
            animator.SetLayerWeight(1, 0);
            Patrol();
        }
    }

    // Chế độ Patrol: boss di chuyển về phía điểm patrol hiện tại
    void Patrol()
    {
        if (!isPatrolActive) return;
        Vector2 direction = (currentPatrolTarget.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * patrolSpeed, rb.velocity.y);
        animator.SetBool("isRunning", true);
        Flip(direction.x);
    }

    // Khi boss chạm vào điểm patrol (với BoxCollider2D có tag "PatrolPoint")
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPatrolActive) return;
        if (other.CompareTag("PatrolPoint"))
        {
            // "Chốt" vị trí: boss được đặt chính xác tại điểm đó và dừng chuyển động
            rb.velocity = Vector2.zero;
            transform.position = other.transform.position;
            StartCoroutine(PatrolWait());
        }
    }

    // Coroutine chờ sau khi boss chạm vào điểm patrol, chuyển sang điểm đối diện và ẩn/hiện các đối tượng
    IEnumerator PatrolWait()
    {
        isPatrolActive = false;
        // Ẩn điểm patrol hiện tại để tránh va chạm liên tục
        currentPatrolTarget.gameObject.SetActive(false);
        yield return new WaitForSeconds(waitTime);

        // Chuyển sang điểm patrol đối diện và hiển thị nó
        if (currentPatrolTarget == rightPoint)
        {
            currentPatrolTarget = leftPoint;
            leftPoint.gameObject.SetActive(true);
        }
        else
        {
            currentPatrolTarget = rightPoint;
            rightPoint.gameObject.SetActive(true);
        }
        // Flip boss theo hướng mới
        Vector2 newDir = (currentPatrolTarget.position - transform.position).normalized;
        Flip(newDir.x);
        isPatrolActive = true;
    }

    // Chế độ Chase: boss đuổi theo player
    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y);
        animator.SetBool("isRunning", true);
        Flip(direction.x);
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

    // Hàm Flip: sử dụng spriteRenderer.flipX để lật sprite theo hướng di chuyển
    void Flip(float moveDirection)
    {
        if (moveDirection > 0)
        {
            spriteRenderer.flipX = false;
            facingRight = true;
        }
        else if (moveDirection < 0)
        {
            spriteRenderer.flipX = true;
            facingRight = false;
        }
    }

    // Vẽ Gizmos để hiển thị vùng detection (chỉ hiển thị trong Scene view khi bật Gizmos)
    void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
}

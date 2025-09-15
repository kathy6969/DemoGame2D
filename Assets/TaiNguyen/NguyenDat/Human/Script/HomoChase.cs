using UnityEngine;

public class HomoChase : MonoBehaviour
{
    public float chaseSpeed = 4f; // Tốc độ đuổi theo người chơi
    public float stopDistance = 1.75f; // Khoảng cách dừng lại trước người chơi
    public float jumpForce = 7f; // Lực nhảy lên khi gặp chướng ngại vật
    public LayerMask obstacleLayer; // Layer chứa chướng ngại vật
    private Transform player;
    private bool isChasing = false;
    private HomoPatrol homoPatrol; // Tham chiếu đến script tuần tra
    private Rigidbody2D rb;
    private Animator animator;
    public GameObject SpriteObject; // Đối tượng con chứa SpriteRenderer

    void Start()
    {
        homoPatrol = GetComponent<HomoPatrol>(); // Lấy tham chiếu đến script tuần tra
        rb = GetComponent<Rigidbody2D>(); // Lấy tham chiếu đến Rigidbody2D
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (isChasing && player != null)
        {
            ChasePlayer(); // Nếu đang đuổi theo thì di chuyển đến vị trí người chơi
        }
    }

    public void StartChase(Transform target)
    {
        if (homoPatrol != null)
        {
            homoPatrol.enabled = false; // Tắt chế độ tuần tra khi phát hiện người chơi
        }
        player = target;
        isChasing = true;
    }

    void ChasePlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Nếu khoảng cách lớn hơn stopDistance, quái mới di chuyển
        if (distanceToPlayer > stopDistance)
        {
            // Kiểm tra có vật cản phía trước không
            if (CheckObstacleInFront())
            {
                // Nếu có chướng ngại vật và có đủ không gian nhảy qua, thực hiện nhảy
                if (CanJumpOverObstacle())
                {
                    Jump();
                }
            }
            else
            {
                // Nếu không có vật cản, di chuyển về phía người chơi
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
                animator.SetBool("move", true);
                animator.SetBool("atk", false);
                FlipTowardsTarget();
            }
        }
        else
        {
            FlipTowardsTarget();
            animator.SetBool("move", false);
            animator.SetBool("atk", true);
        }
    }
    void FlipTowardsTarget()
    {
        if (player != null)
        {
            if (player.position.x > transform.position.x)
            {
                // Người chơi ở bên phải quái
                SpriteObject.transform.localScale = new Vector3(1f, 1f, 1f); // Quái nhìn về bên phải
            }
            else
            {
                // Người chơi ở bên trái quái
                SpriteObject.transform.localScale = new Vector3(-1f, 1f, 1f); // Quái nhìn về bên trái
            }
        }
    }

    // Kiểm tra xem có vật cản phía trước không bằng cách bắn Raycast ngang
    bool CheckObstacleInFront()
    {
        float direction = transform.localScale.x > 0 ? 1f : -1f; // Kiểm tra hướng quái đang nhìn (phải hoặc trái)
        Vector2 rayOrigin = transform.position; // Điểm bắt đầu bắn ray
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * direction, 1f, obstacleLayer);

        // Vẽ Raycast trong Scene để debug
        Debug.DrawRay(rayOrigin, Vector2.right * direction * 1f, Color.red);

        return hit.collider != null; // Nếu có chướng ngại vật phía trước, trả về true
    }

    // Kiểm tra xem quái có thể nhảy qua vật cản không
    bool CanJumpOverObstacle()
    {
        float direction = transform.localScale.x > 0 ? 1f : -1f; // Xác định hướng raycast dựa trên hướng quái đang nhìn
        Vector2 rayOrigin = transform.position + new Vector3(direction * 0f, 1.5f, 0); // Điểm xuất phát của raycast cao hơn quái 1.5 đơn vị
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, 2f, obstacleLayer);

        // Vẽ Raycast trong Scene để debug
        Debug.DrawRay(rayOrigin, Vector2.up * 2f, Color.green);

        return hit.collider == null; // Nếu không có vật cản phía trên, quái có thể nhảy
    }

    // Hàm thực hiện nhảy khi gặp chướng ngại vật
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Tạo lực nhảy lên trên
    }
}

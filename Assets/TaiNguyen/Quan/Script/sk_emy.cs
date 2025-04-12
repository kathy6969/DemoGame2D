using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float attackRange = 5f;
    public float moveSpeed = 3f;

    private Animator animator;
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>(); // Lấy Animator gắn vào quái
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                animator.SetBool("isAttacking", true); // Chuyển sang anim Attack
            }
        }
        else
        {
            if (isAttacking)
            {
                isAttacking = false;
                animator.SetBool("isAttacking", false); // Quay lại anim Idle
            }
        }

        if (isAttacking)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

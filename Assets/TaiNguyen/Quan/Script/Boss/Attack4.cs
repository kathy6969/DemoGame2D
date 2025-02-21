using System.Collections;
using UnityEngine;

public class Attack4 : MonoBehaviour
{
    public float dashSpeed = 10f;  // Tốc độ lướt
    public float maxDashDistance = 10f; // Giới hạn khoảng cách lướt
    public LayerMask groundLayer; // Lớp mặt đất để kiểm tra boss có đứng trên mặt đất không

    private Animator animator;
    private Transform player;
    private bool isDashing = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void PerformAttack()
    {
        if (player == null || isDashing) return;

        float distance = Mathf.Abs(transform.position.x - player.position.x);
        bool isOnGround = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        if (distance <= maxDashDistance && isOnGround) // Kiểm tra điều kiện lướt
        {
            animator.SetTrigger("Attack_glide");
            StartCoroutine(DashToPlayer());
        }
    }

    IEnumerator DashToPlayer()
    {
        isDashing = true;
        Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);

        while (Vector2.Distance(transform.position, targetPosition) > 0.5f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
    }
}

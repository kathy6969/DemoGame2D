using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 10;
    public float lifetime = 5f; // Tự hủy sau 5 giây
    private Transform target; // Người chơi
    private AttackSystem attackSystem;
    public float burnPercentage; // % máu tối đa mất mỗi giây
    public float burnDuration; // Thời gian bị đốt

    void Start()
    {
        Destroy(gameObject, lifetime); // Xóa cầu lửa sau thời gian tồn tại
        attackSystem = GetComponent<AttackSystem>();
        attackSystem.Damage = damage;
    }

    void Update()
    {
        if (target != null)
        {
            // Di chuyển về phía người chơi
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            // Xoay cầu lửa về hướng người chơi
            Vector2 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void SetTarget(Transform player)
    {
        target = player;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HealthSystem playerHealth = other.GetComponent<HealthSystem>();
            playerHealth.ApplyBurn(burnPercentage, burnDuration);
            //Debug.Log("Cầu lửa trúng người chơi!");
            Destroy(gameObject);
        }
    }
}

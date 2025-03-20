using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvFireBall : MonoBehaviour
{
    public float speed = 5f;
    private int damage = 10; // Giá trị mặc định, nhưng sẽ được set từ boss

    private Vector2 direction;

    public float burnPercentage; // % máu tối đa mất mỗi giây
    public float burnDuration; // Thời gian bị đốt
    public float lifetime = 5f; // Tự hủy sau 5 giây
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime); // Xóa cầu lửa sau thời gian tồn tại
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    public void SetDamage(int newDamage) // Nhận sát thương từ boss
    {
        damage = newDamage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            HealthSystem playerHealth = collision.GetComponent<HealthSystem>();
            playerHealth.ApplyBurn(burnPercentage, burnDuration);
        }

        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}

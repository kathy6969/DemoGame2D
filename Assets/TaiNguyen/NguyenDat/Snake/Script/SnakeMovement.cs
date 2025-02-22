using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    public float baseSpeed = 2f;   // Tốc độ cơ bản
    public float maxSpeed = 5f;    // Tốc độ tối đa khi gần hết máu
    public float rayDistance = 0.5f; // Độ dài Raycast để kiểm tra vật cản
    public LayerMask groundLayer;  // Layer của vật cản

    private bool movingRight = true;
    private HealthSystem healthSystem;

    void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    void Update()
    {
        Move();
        CheckObstacle();
    }

    private void Move()
    {
        float currentSpeed = GetSpeedByHealth();
        transform.Translate(Vector2.right * currentSpeed * Time.deltaTime * (movingRight ? 1 : -1));
    }

    private float GetSpeedByHealth()
    {
        if (healthSystem == null) return baseSpeed;

        float healthPercent = healthSystem.TotalHealth / healthSystem.MaxHealth;
        float speedMultiplier = Mathf.Lerp(1f, maxSpeed / baseSpeed, 1 - healthPercent);
        return baseSpeed * speedMultiplier;
    }

    private void CheckObstacle()
    {
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance, groundLayer);

        if (hit.collider != null)
        {
            Flip();
        }
    }

    private void Flip()
    {
        movingRight = !movingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction * rayDistance);
    }
}

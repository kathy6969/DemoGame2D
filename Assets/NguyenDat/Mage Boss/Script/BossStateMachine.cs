using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossStateMachine : MonoBehaviour
{
    private State currentState;
    private TeleportState teleportState;
    private AttackState attackState;
    private IdelState idelState;

    public GameObject fireballPrefab;
    public Transform player;
    public int fireballDamage = 10;
    public float attackRange = 15f; // Khoảng cách để boss chuyển sang trạng thái tấn công
    private AdvFireBall fireballScript;
    public Transform FirePoint;
    public int fireballCount = 5; // Số lượng cầu lửa
    public float fireballSpacing = 2f; // Khoảng cách giữa các cầu lửa
    public float fireballSpawnHeight = 5f; // Chiều cao so với vị trí Boss
    public float fireballFallSpeed = 5f; // Tốc độ rơi

    private void Start()
    {
        teleportState = new TeleportState(this);
        attackState = new AttackState(this, fireballPrefab, player, fireballDamage);
        idelState = new IdelState(this);
        ChangeState(idelState);
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(); // Gọi liên tục để cập nhật trạng thái
        }
        if (player != null && Vector2.Distance(transform.position, player.position) < 5f)
        {
            ChangeState(attackState); // Chuyển sang trạng thái tấn công khi gần người chơi
        }
        else
        {
            ChangeState(teleportState); // Quay lại trạng thái dịch chuyển khi xa
        }
    }
    void ChangeState(State newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }
        else
        {
            currentState = newState;
            currentState.EnterState();
        }
    }
    private void ShootFireball()
    {
        if (fireballPrefab != null && player != null)
        {
            GameObject fireball = Instantiate(fireballPrefab, FirePoint.transform.position, Quaternion.identity);
            fireballScript = fireball.GetComponent<AdvFireBall>();

            if (fireballScript != null)
            {
                fireballScript.SetDamage(fireballDamage); // Set sát thương từ boss
            }

            Vector2 direction = (player.position - FirePoint.transform.position).normalized;
            fireball.GetComponent<Rigidbody2D>().velocity = direction * 5f; // Tốc độ bắn cầu lửa
        }
    }
    private void SummonFireballs()
    {
        Debug.Log("Triệu hồi cầu lửa từ trên cao!");

        float startX = transform.position.x - ((fireballCount - 1) * fireballSpacing / 2); // Điểm đầu để xếp hàng ngang

        for (int i = 0; i < fireballCount; i++)
        {
            Vector3 spawnPosition = new Vector3(startX + i * fireballSpacing, transform.position.y + fireballSpawnHeight, 0);
            GameObject fireball = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);

            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = new Vector2(0, -fireballFallSpeed); // Cho cầu lửa rơi thẳng xuống
            }
        }
    }
}

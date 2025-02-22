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
    public float stateChangeInterval = 3f; // Thời gian đổi trạng thái (3 giây)
    private float stateChangeTimer;

    private void Start()
    {
        teleportState = new TeleportState(this);
        attackState = new AttackState(this, fireballPrefab, player, fireballDamage);
        stateChangeTimer = stateChangeInterval; // Đặt thời gian ban đầu
        SetState(teleportState); // Trạng thái ban đầu là dịch chuyển
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(); // Gọi liên tục để cập nhật trạng thái
        }
        // Đếm thời gian để đổi trạng thái
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance < attackRange)
        {
            Debug.Log("Boss ở gần người chơi, chuyển trạng thái");
            stateChangeTimer -= Time.deltaTime;
            if (stateChangeTimer <= 0)
            {
                ChangeState();
                stateChangeTimer = stateChangeInterval; // Reset bộ đếm
            }
        }
    }
    void ChangeState()
    {
        int randomState = Random.Range(0, 1); // Chọn 1 trong 3 trạng thái

        switch (randomState)
        {
            case 0:
                SetState(new TeleportState(this)); // Dịch chuyển
                break;
            case 1:
                SetState(new AttackState(this, fireballPrefab, player, 10)); // Tấn công
                break;
        }
    }
    public void SetState(State newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(); // Thoát trạng thái hiện tại
        }
        currentState = newState; // Gán trạng thái mới
        currentState.EnterState(); // Kích hoạt trạng thái mới
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

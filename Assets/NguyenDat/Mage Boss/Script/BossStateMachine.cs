using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossStateMachine : MonoBehaviour
{
    private State currentState;
    private TeleportState teleportState;
    private AttackState attackState;

    public GameObject fireballPrefab;
    public Transform player;
    public int fireballDamage = 10;
    public float attackRange = 10f; // Khoảng cách để boss chuyển sang trạng thái tấn công
    private AdvFireBall fireballScript;
    public Transform FirePoint;
    private void Start()
    {
        teleportState = new TeleportState(this);
        attackState = new AttackState(this, fireballPrefab, player, fireballDamage);
        ChangeState(teleportState);
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(); // Gọi liên tục để cập nhật trạng thái
        }
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Nếu người chơi trong phạm vi, boss tấn công
        if (distanceToPlayer < attackRange && !(currentState is AttackState))
        {
            ChangeState(attackState);
        }
        // Nếu không thấy người chơi, chỉ dịch chuyển nếu chưa ở trạng thái dịch chuyển
        else if (distanceToPlayer >= attackRange && !(currentState is TeleportState))
        {
            ChangeState(teleportState);
        }
    }
    public void ChangeState(State newState)
    {
        if (currentState != null)
            currentState.ExitState();

        currentState = newState;
        currentState.EnterState();
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
}

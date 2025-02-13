using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageIdle : MonoBehaviour
{
    private float idleTime = 2f;
    private float timer;
    public float detectionRadius;
    public LayerMask playerLayer;

    private Transform playerTransform;
    private MageAttack mageAttack; // Tham chiếu đến script MageAttack

    void Start()
    {
        timer = idleTime;
        mageAttack = GetComponent<MageAttack>(); // Lấy MageAttack từ cùng GameObject
    }

    void Update()
    {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (player != null)
        {
            if (playerTransform == null)
            {
                playerTransform = player.transform;
                mageAttack.SetTarget(playerTransform); // Gửi thông tin người chơi cho MageAttack
                mageAttack.BeingAttack(); // Bắt đầu tấn công
                Debug.Log("Pháp sư đã phát hiện người chơi và bắt đầu tấn công!");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

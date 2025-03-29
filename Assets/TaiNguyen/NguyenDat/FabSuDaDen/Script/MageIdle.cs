using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageIdle : MonoBehaviour
{
    public float detectionRadius;
    public LayerMask playerLayer;
    public float idleFlipInterval = 2f; // Thời gian giữa mỗi lần tự động lật hướng

    private Transform playerTransform;
    private MageAttack mageAttack;
    private float flipTimer;
    private bool facingRight = true;
    public bool isIdleFlipping = true; // Biến kiểm soát lật hướng

    void Start()
    {
        mageAttack = GetComponent<MageAttack>();
        flipTimer = idleFlipInterval;
    }

    void Update()
    {
        DetectPlayer();
        if (isIdleFlipping)
        {
            HandleIdleFlip();
        }
    }

    private void DetectPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (player != null)
        {
                playerTransform = player.transform;
                mageAttack.SetTarget(playerTransform);
                mageAttack.BeingAttack();
                isIdleFlipping = false; // Dừng tự động lật hướng khi phát hiện Player
               // Debug.Log("Pháp sư đã phát hiện người chơi và bắt đầu tấn công!");
            
        }
        else
        {
                playerTransform = null;
                mageAttack.StopAttack();
                isIdleFlipping = true; // Cho phép tự động lật hướng khi Player rời đi
                //Debug.Log("Pháp sư đã ngừng tấn công, quay lại trạng thái Idle!");
        }
    }

    private void HandleIdleFlip()
    {
        flipTimer -= Time.deltaTime;
        if (flipTimer <= 0)
        {
            Flip();
            flipTimer = idleFlipInterval;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

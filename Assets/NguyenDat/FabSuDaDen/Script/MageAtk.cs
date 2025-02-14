using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAttack : MonoBehaviour
{
    public GameObject fireballPrefab; // Prefab cầu lửa
    public Transform firePoint; // Vị trí bắn cầu lửa

    private Transform playerTransform;
    public Animator animator;
    public float DistanceAttack;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (playerTransform != null)
        {
            FacePlayer(); // Quay mặt về phía người chơi
        }
    }

    private void FacePlayer()
    {
        if (playerTransform != null)
        {
            float directionToPlayer = playerTransform.position.x - transform.position.x;
            if ((directionToPlayer > 0 && transform.localScale.x < 0) || (directionToPlayer < 0 && transform.localScale.x > 0))
            {
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1); // Lật hình khi người chơi đổi hướng
            }
        }
    }

    public void BeingAttack()
    {
        animator.SetBool("atk",true);
    }
    public void StopAttack()
    {
        animator.SetBool("atk", false);
    }

    public void Attack()
    {
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
        Fireball fireballScript = fireball.GetComponent<Fireball>();

        if (fireballScript != null && playerTransform != null)
        {
            fireballScript.SetTarget(playerTransform); // Cầu lửa tự đuổi theo người chơi
        }
    }

    public void SetTarget(Transform player)
    {
        playerTransform = player;
    }
}

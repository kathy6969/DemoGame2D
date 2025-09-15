using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAttack : MonoBehaviour
{
    public GameObject fireballPrefab; // Prefab cầu lửa
    public Transform firePoint; // Vị trí bắn cầu lửa

    private Transform playerTransform;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        if (playerTransform.position.x < transform.position.x)
        {
            // Người chơi ở bên trái
            spriteRenderer.flipX = true; // Lật sprite sang trái
            firePoint.localPosition = new Vector3(-Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, firePoint.localPosition.z);
        }
        else
        {
            // Người chơi ở bên phải
            spriteRenderer.flipX = false; // Lật sprite sang phải
            firePoint.localPosition = new Vector3(Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, firePoint.localPosition.z);
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

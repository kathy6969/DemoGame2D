using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomoPatrol : MonoBehaviour
{
    public float patrolDistance = 5f;
    public float speed = 2f;
    public float waitTime = 2f;

    private Vector2 startPosition;
    public bool movingRight = true;
    private bool isWaiting = false;
    private Animator animator;
    private RaycastShooter raycastShooter;

    [Header("Sprite Object")]
    public GameObject spriteObject; // Tham chiếu tới object con chứa sprite

    void Start()
    {
        startPosition = transform.position;
        animator = GetComponentInChildren<Animator>();
        raycastShooter = GetComponent<RaycastShooter>();
        // Nếu chưa gán spriteObject, tự động tìm theo tên
        if (spriteObject == null)
        {
            spriteObject = transform.Find("spriteObject")?.gameObject;
        }
    }

    void Update()
    {
        if (!isWaiting)
        {
            Patrol();
            animator.SetBool("move", true);
        }
        else
        {
            animator.SetBool("move", false);
        }
    }
    void Patrol()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            if (transform.position.x >= startPosition.x + patrolDistance)
            {
                StartCoroutine(WaitAndFlip());
            }
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            if (transform.position.x <= startPosition.x - patrolDistance)
            {
                StartCoroutine(WaitAndFlip());
            }
        }
    }

    IEnumerator WaitAndFlip()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        movingRight = !movingRight;
        Flip();
        isWaiting = false;
    }

    void Flip()
    {
        if (raycastShooter != null)
        {
            raycastShooter.Flip();
        }
        // Lật hình ảnh bằng cách lật object con spriteObject
        if (spriteObject != null)
        {
            Vector3 scale = spriteObject.transform.localScale;
            scale.x *= -1;
            spriteObject.transform.localScale = scale;
        }
    }
}

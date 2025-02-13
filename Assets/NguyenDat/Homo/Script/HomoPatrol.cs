using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomoPatrol : MonoBehaviour
{
    public float patrolDistance = 5f; // Khoảng cách di chuyển tối đa từ vị trí ban đầu
    public float speed = 2f; // Tốc độ di chuyển
    public float waitTime = 2f; // Thời gian nghỉ khi đến giới hạn

    private Vector2 startPosition;
    public bool movingRight = true;
    private bool isWaiting = false; // Kiểm tra xem quái có đang nghỉ không
    private Animator animator;
    private RaycastShooter raycastShooter;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;// Lưu lại vị trí ban đầu
        animator = GetComponent<Animator>();
        raycastShooter = GetComponent<RaycastShooter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWaiting)
        {
            Patrol(); // Chỉ di chuyển khi không ở trạng thái nghỉ
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
            transform.position += Vector3.right * speed * Time.deltaTime; // Di chuyển sang phải
            if (transform.position.x >= startPosition.x + patrolDistance)
            {
                StartCoroutine(WaitAndFlip()); // Bắt đầu nghỉ trước khi đổi hướng
            }
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime; // Di chuyển sang trái
            if (transform.position.x <= startPosition.x - patrolDistance)
            {
                StartCoroutine(WaitAndFlip()); // Bắt đầu nghỉ trước khi đổi hướng
            }
        }
    }

    IEnumerator WaitAndFlip()
    {
        isWaiting = true; // Đánh dấu quái đang nghỉ
        yield return new WaitForSeconds(waitTime); // Chờ trong khoảng thời gian đã đặt
        movingRight = !movingRight; // Đảo hướng di chuyển
        Flip(); // Đảo hướng hình ảnh
        isWaiting = false; // Quái tiếp tục di chuyển
    }

    void Flip()
    {
        // Gọi RaycastShooter để lật hướng ray trước
        if (raycastShooter != null)
        {
            raycastShooter.Flip();
        }
        // Đảo hướng hình ảnh
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}

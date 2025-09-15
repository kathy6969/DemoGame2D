using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 5f; // Tốc độ di chuyển ngang
    public float moveDistance = 5f; // Khoảng cách di chuyển ngang từ vị trí ban đầu
    public float waveHeight = 1f; // Độ cao của sóng
    public float waveSpeed = 2f; // Tốc độ lên xuống của sóng

    private float leftBound;
    private float rightBound;
    private bool movingLeft = true; // Trạng thái di chuyển
    private SpriteRenderer spriteRenderer;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        // Lưu vị trí ban đầu
        startPos = transform.position;

        // Xác định giới hạn di chuyển theo phương ngang
        leftBound = startPos.x - moveDistance;
        rightBound = startPos.x + moveDistance;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); // Lấy SpriteRenderer để lật ảnh
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    void Movement()
    {
        // Xác định hướng di chuyển ngang
        float moveDirection = movingLeft ? -1 : 1;
        float newX = transform.position.x + moveDirection * speed * Time.deltaTime;

        // Tính toán chuyển động sóng sin theo trục Y
        float newY = startPos.y + Mathf.Sin(Time.time * waveSpeed) * waveHeight;

        // Cập nhật vị trí
        transform.position = new Vector3(newX, newY, transform.position.z);

        // Kiểm tra nếu chạm giới hạn
        if (newX <= leftBound)
        {
            movingLeft = false;
            Flip();
        }
        else if (newX >= rightBound)
        {
            movingLeft = true;
            Flip();
        }
    }

    void Flip()
    {
        // Lật hình ảnh con dơi
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}

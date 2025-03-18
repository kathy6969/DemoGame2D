using UnityEngine;

public class CoinProjectile : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 moveDirection;
    void Start()
    {
        Destroy(gameObject, 3f);
    }
    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized; // Chuẩn hóa hướng đi
    }

    void Update()
    {
        transform.position += (Vector3)moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) // Kiểm tra nếu trúng kẻ địch
        {
            Destroy(gameObject); // Xóa đồng xu sau khi trúng
        }
    }
}

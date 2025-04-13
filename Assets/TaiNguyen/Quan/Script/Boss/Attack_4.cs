using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject bulletPrefab;       // Prefab của viên đạn
    public Transform firePoint;           // Vị trí bắn ra (điểm spawn của viên đạn)
    public float bulletSpeed = 5f;        // Tốc độ bay của đạn
    public SpriteRenderer spriteRenderer; // Tham chiếu đến SpriteRenderer của enemy

    private Vector3 rightFirePos = new Vector3(1.47f, -1.40f, 0f);  // Khi nhìn phải
    private Vector3 leftFirePos = new Vector3(-1.47f, -1.4055f, 0f);  // Khi nhìn trái

    public void Shoot()
    {
        // Xác định hướng bắn dựa trên spriteRenderer.flipX.
        float direction = (spriteRenderer != null && spriteRenderer.flipX) ? -1f : 1f;

        // Đặt lại vị trí firePoint theo hướng nhìn
        firePoint.localPosition = (direction == -1f) ? leftFirePos : rightFirePos;

        // Xoay đạn nếu cần
        Quaternion bulletRotation = (direction < 0) ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.identity;

        // Tạo đạn
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);

        // Thêm vận tốc cho đạn
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = new Vector2(direction * bulletSpeed, 0f);
        }

        // Tự hủy sau 5 giây
        Destroy(bullet, 5f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Shoot();
        }
    }
}

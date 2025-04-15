using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint; // FirePoint để xác định hướng bắn
    private PlayerRotation playerRotation; // Tham chiếu đến PlayerRotation script
    public static bool Shot;
    public float timeBetweenShots = 1f; // Thời gian giữa mỗi lần bắn (1 giây)
    private float lastShotTime = 0f; // Lưu thời gian bắn lần cuối
    void Start()
    {
        playerRotation = GetComponent<PlayerRotation>(); // Lấy tham chiếu đến PlayerRotation
    }

    void Update()
    {
        // Kiểm tra nếu chuột trái được nhấn và nếu thời gian đã trôi qua ít nhất 1 giây kể từ lần bắn trước
        if (Input.GetMouseButtonDown(0) && Time.time >= lastShotTime + timeBetweenShots) 
        {
            Shot = true; // Player bắt đầu bắn
            Shoot(); // Gọi hàm bắn
            lastShotTime = Time.time; // Cập nhật thời gian bắn lần cuối
        }
        // Khi nhả chuột
        if (Input.GetMouseButtonUp(0)) 
        {
            Shot = false; // Player ngừng bắn
        }
    }

    public void Shoot()
    {
        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; 
        Vector2 direction = (mousePosition - firePoint.transform.position).normalized;
        Bullet bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.SetDirection(direction);
    }
}
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Nhấn chuột trái
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 direction = (mousePosition - transform.position).normalized;

        // Xác định hướng bắn để quay Player và FirePoint
        if (direction.x > 0)
        {
            spriteRenderer.flipX = false; // Quay phải
            firePoint.localPosition = new Vector3(Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, 0);
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true; // Quay trái
            firePoint.localPosition = new Vector3(-Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, 0);
        }

        Bullet bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = firePoint.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        bullet.SetDirection(direction);
    }
}
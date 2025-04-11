using UnityEngine;

public class AttackTester : MonoBehaviour
{
    public GameObject bullet2;
    public GameObject bullet4;
    public GameObject bullet5;
    public GameObject bullet6;
    public GameObject bullet7;
    public GameObject bullet8;

    public Transform firePoint;

    public float bulletSpeed = 10f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Shoot(bullet2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Shoot(bullet4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Shoot(bullet5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Shoot(bullet6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Shoot(bullet7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Shoot(bullet8);
        }
    }

    void Shoot(GameObject bulletPrefab)
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = firePoint.right * bulletSpeed;
        }
        Destroy(bullet, 5f); // tự huỷ sau 5 giây
    }
}

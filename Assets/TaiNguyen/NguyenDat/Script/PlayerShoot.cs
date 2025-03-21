using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject coinPrefab;
    public Transform firePoint;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Nhấn chuột trái để bắn
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Đảm bảo Z = 0 (vì 2D)

        Vector2 shootDirection = (mousePosition - firePoint.position).normalized; // Hướng bắn

        GameObject coin = Instantiate(coinPrefab, firePoint.position, Quaternion.identity);
        coin.GetComponent<CoinProjectile>().SetDirection(shootDirection);
    }
}

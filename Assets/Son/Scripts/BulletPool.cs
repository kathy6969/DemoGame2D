using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance; // Singleton để dễ truy cập

    public Bullet bulletPrefab;
    public int poolSize = 10;
    private Queue<Bullet> bulletPool = new Queue<Bullet>();

    private Transform playerTransform; // Tham chiếu đến Player

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform; // Tìm Player
        if (playerTransform == null)
        {
            Debug.LogError("Không tìm thấy Player! Hãy đảm bảo Player có tag 'Player'");
            return;
        }

        for (int i = 0; i < poolSize; i++)
        {
            Bullet bullet = Instantiate(bulletPrefab, playerTransform);
            bullet.gameObject.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }

    public Bullet GetBullet()
    {
        Bullet bullet;
        if (bulletPool.Count > 0)
        {
            bullet = bulletPool.Dequeue();
        }
        else
        {
            bullet = Instantiate(bulletPrefab, playerTransform);
        }

        bullet.transform.SetParent(playerTransform); // Đặt viên đạn là con của Player
        bullet.gameObject.SetActive(true);
        return bullet;
    }

    public void ReturnBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(playerTransform); // Đảm bảo viên đạn về Player khi trở lại pool
        bulletPool.Enqueue(bullet);
    }
}
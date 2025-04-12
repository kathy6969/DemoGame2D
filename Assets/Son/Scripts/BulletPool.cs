using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance; // Singleton

    public Bullet bulletPrefab;
    public int poolSize = 10;
    private Queue<Bullet> bulletPool = new Queue<Bullet>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Bullet bullet = Instantiate(bulletPrefab, transform); // Đặt viên đạn là con của BulletPool
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
            bullet = Instantiate(bulletPrefab, transform); // Đặt viên đạn là con của BulletPool
        }

        bullet.transform.SetParent(transform); // Đảm bảo viên đạn vẫn là con của BulletPool
        bullet.gameObject.SetActive(true);
        return bullet;
    }

    public void ReturnBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(transform); // Đặt viên đạn trở lại BulletPool
        bulletPool.Enqueue(bullet);
    }
}
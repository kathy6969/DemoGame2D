using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance; // Singleton để dễ truy cập

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
            Bullet bullet = Instantiate(bulletPrefab);
            bullet.gameObject.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }

    public Bullet GetBullet()
    {
        if (bulletPool.Count > 0)
        {
            Bullet bullet = bulletPool.Dequeue();
            bullet.gameObject.SetActive(true);
            return bullet;
        }
        else
        {
            Bullet newBullet = Instantiate(bulletPrefab);
            return newBullet;
        }
    }

    public void ReturnBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}
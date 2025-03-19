using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;

    private Vector2 moveDirection;
    private float timer;
    private Camera mainCamera;
    
    void Awake()
    {
        mainCamera = Camera.main;
    }

    void OnEnable()
    {
        timer = lifeTime;
    }

    void Update()
    {
        transform.position += (Vector3)moveDirection * speed * Time.deltaTime;

        timer -= Time.deltaTime;
        if (timer <= 0 || IsOutOfCameraView())
        {
            BulletPool.Instance.ReturnBullet(this);
        }
    }

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            BulletPool.Instance.ReturnBullet(this);
        }
    }

    private bool IsOutOfCameraView()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        return screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1;
    }
}

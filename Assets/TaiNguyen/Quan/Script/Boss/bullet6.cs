using UnityEngine;

public class Fireball6 : MonoBehaviour
{
    private void Start()
    {
        // Tự động huỷ sau 5 giây để tránh tồn tại mãi
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Nếu chạm vào Ground thì huỷ đạn
        if (collision.collider.CompareTag("ground"))
        {
            Destroy(gameObject);
        }
    }
}

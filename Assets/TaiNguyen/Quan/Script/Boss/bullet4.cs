using UnityEngine;

public class FireOrbBullet : MonoBehaviour
{
    public float lifetime = 5f; // Thời gian sống của đạn

    void Start()
    {
        // Tự hủy sau lifetime giây
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            Destroy(gameObject);
        }
    }
}

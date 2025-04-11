using UnityEngine;

public class BulletSimple : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime); // Tự hủy sau 5s
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime); // Bay thẳng theo trục X
    }
}

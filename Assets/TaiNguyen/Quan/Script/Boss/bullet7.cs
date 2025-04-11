using UnityEngine;

public class bullet7 : MonoBehaviour
{
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // Tự hủy sau 5 giây
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Nếu chạm vào Player hoặc Ground thì hủy đạn
       
    }
}

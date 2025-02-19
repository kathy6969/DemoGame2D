using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Khi Player chạm vào Coin (Trigger)
        {
            GameManager.instance.IncreaseScore(1); // Tăng điểm
            Destroy(gameObject); // Xóa Coin khỏi Scene
        }
    }
}
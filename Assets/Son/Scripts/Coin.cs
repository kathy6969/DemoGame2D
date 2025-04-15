using UnityEngine;

public class Coin : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu Player va chạm với Coin
        if (other.CompareTag("Player"))
        {
            TextCoin.Instance.AddCoin(1);
            Destroy(gameObject);
        }
    }
}
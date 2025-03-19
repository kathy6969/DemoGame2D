using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Số máu hồi phục khi va chạm
    public float healthAmount = 20f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra xem đối tượng va chạm có phải là Player không và có tag "Player"
        if (collision.collider.CompareTag("Player"))
        {
            // Truyền máu vào cho Player (giả sử PlayerHP là singleton)
            PlayerHP.Instance.AddHealth(healthAmount);

            // Destroy Item sau khi va chạm (nếu cần)
            Destroy(gameObject);
        }
    }
}
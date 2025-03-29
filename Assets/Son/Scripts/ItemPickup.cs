using UnityEngine;

public class ItemPickup : MonoBehaviour

{

    // Số máu hồi phục khi va chạm
    public float healthAmount = 20f;
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    // Kiểm tra xem đối tượng va chạm có phải là Player không và có tag "Player"
        
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealthSystem healthSystem = collision.GetComponent<HealthSystem>();

            if (healthSystem != null)
            {
                // Gọi phương thức trên HealthSystem
                healthSystem.DamageTake(-healthAmount);
                
            }
         
            // Destroy Item sau khi va chạm (nếu cần)
            Destroy(gameObject);
        }
    }

}
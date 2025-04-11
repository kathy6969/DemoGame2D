using UnityEngine;

public class Attack_ShootAtPlayer : MonoBehaviour
{
    public GameObject fireballPrefab;
    public float fireballSpeed = 7f;
    public Transform spawnPoint; // Nếu null sẽ dùng vị trí hiện tại

    public void ExecuteAttack()
    {
        if (spawnPoint == null) spawnPoint = transform;

        // Tìm player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        // Tính hướng bay
        Vector2 direction = (player.transform.position - spawnPoint.position).normalized;

        // Tạo đạn
        GameObject fireball = Instantiate(fireballPrefab, spawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * fireballSpeed;
        }

        Destroy(fireball, 5f); // Tự hủy sau 5s
    }
}

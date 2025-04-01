using UnityEngine;

public class Attack7_ShootFireball : MonoBehaviour
{
    public GameObject fireballPlayerPrefab;
    public float fireballPlayerSpeed = 7f;
    public Transform fireballPlayerSpawnPoint; // Nếu không gán, sử dụng vị trí của object chứa script

    public void ExecuteAttack7()
    {
        if (fireballPlayerSpawnPoint == null)
            fireballPlayerSpawnPoint = transform;
        GameObject fireball = Instantiate(fireballPlayerPrefab, fireballPlayerSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Hướng tấn công hướng đến player (giả sử có tag "Player")
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                Vector2 direction = (playerObj.transform.position - fireballPlayerSpawnPoint.position).normalized;
                rb.velocity = direction * fireballPlayerSpeed;
            }
        }
    }
}

using UnityEngine;

public class Attack4_FireOrbStraight : MonoBehaviour
{
    [Header("Prefab & Settings")]
    public GameObject fireOrbStraightPrefab; // Viên đạn (phải là prefab)
    public float fireOrbStraightSpeed = 5f;

    [Header("Vị trí sinh đạn")]
    public Transform fireOrbSpawnPoint; // Điểm đặt thủ công trong scene

    // Hàm này sẽ gọi trong Animation Event
    public void ExecuteAttack4()
    {
        if (fireOrbStraightPrefab == null)
        {
            Debug.LogError("Chưa gán Prefab đạn!");
            return;
        }

        if (fireOrbSpawnPoint == null)
        {
            Debug.LogError("Chưa gán fireOrbSpawnPoint!");
            return;
        }

        // Tạo đạn tại vị trí spawn
        GameObject orb = Instantiate(fireOrbStraightPrefab, fireOrbSpawnPoint.position, Quaternion.identity);

        // Thiết lập hướng bay dựa vào hướng Boss (scale.x)
        Rigidbody2D rb = orb.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            rb.velocity = direction * fireOrbStraightSpeed;
        }

        // Tự hủy sau 5s
        Destroy(orb, 5f);
    }
}

using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public GameObject fireOrbPrefab; // Prefab của viên đạn
    public Transform firePoint;      // Vị trí bắn ra

    public void Attack4()
    {
        // Tạo đạn tại vị trí firePoint
        GameObject orb = Instantiate(fireOrbPrefab, firePoint.position, Quaternion.identity);

        // Xác định hướng bắn dựa theo hướng boss đang nhìn
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        // Gọi hàm Launch của viên đạn để bắn theo hướng
        orb.GetComponent<FireOrb>().Launch(direction);
    }
}

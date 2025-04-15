using UnityEngine;

public class BossAttack4 : MonoBehaviour
{
    public GameObject fireOrbPrefab;   // Prefab của viên đạn
    public Transform firePoint;        // Vị trí bắt đầu bắn viên đạn
    public float attackCooldown = 2f;  // Thời gian chờ giữa các lần sử dụng attack

    private float nextAttackTime = 0f;

    void Update()
    {
        // Kiểm tra khi bấm phím (ở đây là phím "F" để thực hiện Attack 4)
        if (Input.GetKeyDown(KeyCode.F) && Time.time >= nextAttackTime)
        {
            // Thực hiện Attack 4
            Attack4();

            // Cập nhật thời gian cooldown
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void Attack4()
    {
        // Tạo viên đạn và gọi chức năng launch
        GameObject orb = Instantiate(fireOrbPrefab, firePoint.position, Quaternion.identity);

        // Xác định hướng viên đạn bay dựa trên hướng nhìn của Boss
        Vector2 direction = Vector2.zero;

        if (transform.localScale.x > 0)  // Boss nhìn sang phải
        {
            direction = Vector2.right;
        }
        else if (transform.localScale.x < 0)  // Boss nhìn sang trái
        {
            direction = Vector2.left;
        }

        // Bắn viên đạn theo hướng tính toán
        orb.GetComponent<FireOrb>().Launch(direction);

        // Optionally: Add a visual or animation effect to show the attack
    }
}

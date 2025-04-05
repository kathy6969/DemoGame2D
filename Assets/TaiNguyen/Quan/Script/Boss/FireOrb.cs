using UnityEngine;

public class FireOrbOrbit : MonoBehaviour
{
    [Tooltip("Vị trí trung tâm của quỹ đạo (boss)")]
    public Transform center;
    [Tooltip("Tốc độ quay, tính bằng độ/giây")]
    public float orbitSpeed = 90f; // 90 độ/giây

    private Vector3 offset;

    void Start()
    {
        // Tự hủy sau 5 giây
        Destroy(gameObject, 5f);

        // Nếu chưa gán center từ Inspector, tìm boss theo tag "Boss"
        if (center == null)
        {
            GameObject bossObj = GameObject.FindWithTag("boss");
            if (bossObj != null)
            {
                center = bossObj.transform;
            }
            else
            {
                Debug.LogWarning("Không tìm thấy boss với tag 'Boss'.");
            }
        }

        if (center != null)
        {
            // Lưu khoảng cách ban đầu từ fire orb tới center
            offset = transform.position - center.position;
        }
    }

    void Update()
    {
        if (center != null)
        {
            // Xoay offset quanh trục Z (vì game 2D)
            offset = Quaternion.Euler(0, 0, orbitSpeed * Time.deltaTime) * offset;
            transform.position = center.position + offset;
        }
    }
}

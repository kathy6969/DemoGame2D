using UnityEngine;

public class FireOrbOrbit : MonoBehaviour
{
    [Tooltip("Vị trí trung tâm của quỹ đạo (boss)")]
    public Transform center;

    [Tooltip("Tốc độ quay, tính bằng độ/giây")]
    public float orbitSpeed = 90f;

    [Tooltip("Hệ số nhân khoảng cách ban đầu để tạo quỹ đạo xa hơn")]
    public float orbitRadiusMultiplier = 2f; // 👈 Tăng giá trị này để đạn quay xa hơn

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
            // Nhân khoảng cách với hệ số để tăng bán kính quay
            offset = (transform.position - center.position) * orbitRadiusMultiplier;
        }
    }

    void Update()
    {
        if (center != null)
        {
            // Xoay offset quanh trục Z
            offset = Quaternion.Euler(0, 0, orbitSpeed * Time.deltaTime) * offset;
            transform.position = center.position + offset;
        }
    }
}

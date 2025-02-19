using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarFollow : MonoBehaviour
{
    public Transform player; // Kéo thả Player vào đây
    public Vector3 offset = new Vector3(0, 2f, 0); // Điều chỉnh vị trí thanh máu
    private RectTransform rectTransform;
    public float smoothSpeed = 0.125f; // Tốc độ mượt (có thể điều chỉnh)

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();  // Lấy RectTransform của Canvas
    }

    void Update() // Đồng bộ với Cinemachine
    {
        if (player != null)
        {
            // Nếu Canvas ở chế độ UI Screen Space (Overlay hoặc Camera)
            if (rectTransform != null)
            {
                // Chuyển vị trí player từ world space sang screen space
                Vector3 targetPosition = Camera.main.WorldToScreenPoint(player.position + offset);

                // Di chuyển mượt mà đến vị trí mục tiêu
                rectTransform.position = Vector3.Lerp(rectTransform.position, targetPosition, smoothSpeed);
            }
            // Nếu Canvas ở chế độ World Space
            else
            {
                // Cập nhật vị trí của Canvas theo player với offset
                transform.position = player.position + offset;

                // Giữ rotation cố định (không xoay)
                transform.rotation = Quaternion.identity; // Không thay đổi góc quay
            }
        }
    }
}
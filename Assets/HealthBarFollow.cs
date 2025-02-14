using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarFollow : MonoBehaviour
{
    public Transform player; // Kéo thả Player vào đây
    public Vector3 offset = new Vector3(0, 0f, 0); // Điều chỉnh vị trí thanh máu
    private RectTransform rectTransform;
    private Quaternion initialRotation;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialRotation = transform.rotation; // Lưu góc quay ban đầu
    }

    void Update() // Đồng bộ với Cinemachine
    {
        if (player != null)
        {
            // Nếu dùng UI Screen Space - Camera hoặc Overlay
            if (rectTransform != null)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(player.position + offset);
                rectTransform.position = screenPos;
            }
            else // Nếu dùng World Space
            {
                transform.position = player.position + offset;
                transform.rotation = initialRotation; // Giữ nguyên góc quay
            }
        }
    }
}

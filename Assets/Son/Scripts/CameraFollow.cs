using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Nhân vật cần theo dõi
    public Vector3 offset = new Vector3(0, 0, -10); // Khoảng cách camera
    public float smoothSpeed = 0.125f; // Tốc độ mượt (nếu cần)

    void Update()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position + offset;
            transform.position = targetPosition; // Luôn theo sát nhân vật, không bị trễ
        }
    }
}
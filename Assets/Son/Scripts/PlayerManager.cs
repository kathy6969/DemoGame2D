using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject); // Giữ Player không bị hủy khi chuyển cảnh
    }
}
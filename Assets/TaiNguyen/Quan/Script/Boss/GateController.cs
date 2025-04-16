using UnityEngine;
using UnityEngine.Tilemaps;

public class BossChecker : MonoBehaviour
{
    [Header("Cài đặt Boss")]
    [SerializeField] private string bossTag = "Boss"; // Tag của boss
    [SerializeField] private float checkRate = 0.5f; // Tần suất kiểm tra (giây)

    [Header("Thay đổi Tilemap")]
    [SerializeField] private Tilemap targetTilemap;
    [SerializeField] private Collider2D targetCollider;

    private Rigidbody2D tilemapRigidbody;
    private float checkTimer;

    private void Start()
    {
        // Kiểm tra component
        if (targetTilemap != null)
        {
            tilemapRigidbody = targetTilemap.GetComponent<Rigidbody2D>();
            if (tilemapRigidbody == null)
            {
                Debug.LogWarning("Tilemap không có Rigidbody2D, sẽ không thể thay đổi thành Dynamic");
            }
        }

        checkTimer = checkRate;
    }

    private void Update()
    {
        checkTimer -= Time.deltaTime;
        if (checkTimer <= 0f)
        {
            CheckBoss();
            checkTimer = checkRate;
        }
    }

    private void CheckBoss()
    {
        // Tìm boss theo tag
        GameObject boss = GameObject.FindWithTag(bossTag);

        // Nếu không tìm thấy boss
        if (boss == null)
        {
            ActivateChanges();
            enabled = false; // Tắt script sau khi kích hoạt thay đổi
        }
    }

    private void ActivateChanges()
    {
        // Thay đổi Rigidbody thành Dynamic
        if (tilemapRigidbody != null)
        {
            tilemapRigidbody.bodyType = RigidbodyType2D.Dynamic;
            Debug.Log("Đã thay đổi Tilemap sang Dynamic");
        }

        // Bật Is Trigger cho collider
        if (targetCollider != null)
        {
            targetCollider.isTrigger = true;
            Debug.Log("Đã bật Is Trigger cho collider");
        }
    }
}
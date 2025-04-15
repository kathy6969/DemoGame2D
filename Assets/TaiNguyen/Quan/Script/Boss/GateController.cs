using UnityEngine;

public class GateController : MonoBehaviour
{
    [Header("Cổng điều khiển")]
    public GameObject gate1;
    public GameObject gate2;
    public GameObject boss; // Boss liên quan đến cổng này

    private bool playerBetweenGates = false;
    private bool bossDefeated = false;
    private Collider2D gate1Collider;
    private Collider2D gate2Collider;

    void Start()
    {
        // Lấy Collider của các cổng
        gate1Collider = gate1.GetComponent<Collider2D>();
        gate2Collider = gate2.GetComponent<Collider2D>();

        // Mặc định mở cổng (isTrigger = true)
        SetGatesTrigger(true);
    }

    void Update()
    {
        // Kiểm tra nếu boss đã bị tiêu diệt
        if (!bossDefeated && boss == null)
        {
            bossDefeated = true;
            DisableGatesPermanently();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (IsBetweenGates(other.transform.position))
            {
                playerBetweenGates = true;
                SetGatesTrigger(false); // Đóng cổng
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerBetweenGates)
        {
            playerBetweenGates = false;
            if (!bossDefeated)
            {
                SetGatesTrigger(true); // Mở cổng nếu boss chưa chết
            }
        }
    }

    private bool IsBetweenGates(Vector2 position)
    {
        // Kiểm tra vị trí player có nằm giữa hai cổng không
        float minX = Mathf.Min(gate1.transform.position.x, gate2.transform.position.x);
        float maxX = Mathf.Max(gate1.transform.position.x, gate2.transform.position.x);
        return position.x > minX && position.x < maxX;
    }

    private void SetGatesTrigger(bool isTrigger)
    {
        gate1Collider.isTrigger = isTrigger;
        gate2Collider.isTrigger = isTrigger;
    }

    private void DisableGatesPermanently()
    {
        // Vô hiệu hóa hoàn toàn cổng sau khi boss chết
        gate1Collider.enabled = false;
        gate2Collider.enabled = false;

        // Có thể thêm hiệu ứng hoặc ẩn cổng tùy ý
        gate1.SetActive(false);
        gate2.SetActive(false);
    }
}
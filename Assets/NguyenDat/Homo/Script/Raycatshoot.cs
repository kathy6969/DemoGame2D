using UnityEngine;

public class RaycastShooter : MonoBehaviour
{
    public Transform raycastPoint; // Điểm bắt đầu bắn raycast
    public float rayDistance = 10f; // Khoảng cách bắn raycast
    public LayerMask hitLayers; // Lớp có thể bị raycast chạm vào
    private bool facingRight = true; // Biến kiểm tra hướng của quái
    private HomoChase homoChase;

    void Start()
    {
        homoChase = GetComponent<HomoChase>();
    }

    void Update()
    {
        ShootRay();
    }

    void ShootRay()
    {
        if (raycastPoint == null)
        {
            Debug.LogWarning("Raycast Point is not assigned!");
            return;
        }

        // Xác định hướng bắn ray dựa trên hướng hiện tại của quái
        Vector2 rayDirection = facingRight ? Vector2.right : Vector2.left;

        // Bắn một ray từ vị trí raycastPoint theo hướng hiện tại
        RaycastHit2D hit = Physics2D.Raycast(raycastPoint.position, rayDirection, rayDistance, hitLayers);

        // Vẽ đường ray trong Scene để dễ debug
        Debug.DrawRay(raycastPoint.position, rayDirection * rayDistance, Color.red);

        //Kiểm tra nếu ray chạm vào vật thể nào đó
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            //Debug.Log("Hit: " + hit.collider.name);
            if (homoChase != null)
            {
                homoChase.StartChase(hit.collider.transform);
            }
        }
    }

    // Hàm này được gọi từ script khác khi quái lật hướng
    public void Flip()
    {
        facingRight = !facingRight;
    }
}

using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public bool facingRight = true; // Biến kiểm tra player đang quay hướng nào

    // Quay player theo hướng trái/phải
    public void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Quay firePoint theo hướng bắn (nếu có)
    public void RotateToDirection(Vector2 direction)
    {
        // Quay mặt của player theo hướng bắn (chỉ quay mặt chứ không phải toàn bộ player)
        if (direction.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && facingRight)
        {
            Flip();
        }
    }
}
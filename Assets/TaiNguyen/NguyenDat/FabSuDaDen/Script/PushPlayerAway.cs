using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPlayerAway : MonoBehaviour
{
    public float pushRadius = 1.5f; // Bán kính phát hiện người chơi
    public float pushForce = 10f;   // Lực đẩy
    public float disableMoveTime = 0.3f; // Thời gian vô hiệu hóa điều khiển
    public LayerMask playerLayer;   // Layer của người chơi

    private void Update()
    {
        PushPlayer();
    }

    private void PushPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, pushRadius, playerLayer);

        if (playerCollider != null)
        {
            Rigidbody2D playerRb = playerCollider.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                Debug.Log("Đã đẩy Player ra xa!");

                // Xóa vận tốc cũ trước khi đẩy
                playerRb.velocity = Vector2.zero;

                // Xác định hướng đẩy (chỉ theo trục X)
                Vector2 pushDirection = (playerRb.transform.position - transform.position).normalized;
                pushDirection.y = 0; // Không đẩy lên trời

                // Dùng AddForce để đẩy ngay lập tức
                playerRb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);

                // Tìm và tắt tạm thời script điều khiển của Player
                MonoBehaviour playerControlScript = playerCollider.GetComponent<MonoBehaviour>();

                if (playerControlScript != null)
                {
                    playerControlScript.enabled = false;
                    StartCoroutine(EnablePlayerControl(playerControlScript, disableMoveTime));
                }
            }
        }
    }

    private IEnumerator EnablePlayerControl(MonoBehaviour playerControlScript, float delay)
    {
        yield return new WaitForSeconds(delay);
        playerControlScript.enabled = true; // Bật lại script điều khiển
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pushRadius); // Hiển thị phạm vi đẩy trong Editor
    }
}

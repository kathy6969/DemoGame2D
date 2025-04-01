using UnityEngine;

public class Attack4_FireOrbStraight : MonoBehaviour
{
    public GameObject fireOrbStraightPrefab;
    public float fireOrbStraightSpeed = 5f;
    public Transform fireOrbSpawnPoint; // Nếu không gán, sử dụng vị trí của object chứa script

    public void ExecuteAttack4()
    {
        if (fireOrbSpawnPoint == null)
            fireOrbSpawnPoint = transform;
        GameObject orb = Instantiate(fireOrbStraightPrefab, fireOrbSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = orb.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Hướng được quyết định dựa trên scale.x (flip) của object chứa script
            Vector2 direction = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;
            rb.velocity = direction * fireOrbStraightSpeed;
        }
    }
}

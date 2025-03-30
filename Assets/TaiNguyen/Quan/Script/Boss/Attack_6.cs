using UnityEngine;

public class Attack6_FireballsFromAbove : MonoBehaviour
{
    public GameObject fireballPrefab;
    public int fireballCount = 6;
    public float fireballHorizontalSpacing = 4f;
    public float fireballSpawnHeightOffset = 10f;

    public void ExecuteAttack6()
    {
        float startX = transform.position.x - (fireballCount - 1) * fireballHorizontalSpacing / 2f;
        for (int i = 0; i < fireballCount; i++)
        {
            Vector3 spawnPos = new Vector3(startX + i * fireballHorizontalSpacing, transform.position.y + fireballSpawnHeightOffset, transform.position.z);
            Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
        }
    }
}

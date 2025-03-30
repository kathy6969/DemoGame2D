using UnityEngine;

public class Attack2_FireOrbs : MonoBehaviour
{
    public GameObject fireOrbPrefab;
    public int orbCount = 6;
    public float orbRadius = 2f;

    // Gọi qua Animation Event
    public void ExecuteAttack2()
    {
        for (int i = 0; i < orbCount; i++)
        {
            float angle = i * Mathf.PI * 2f / orbCount;
            Vector3 spawnPos = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * orbRadius;
            Instantiate(fireOrbPrefab, spawnPos, Quaternion.identity);
        }
    }
}

using UnityEngine;

public class Attack5_SummonSkeletons : MonoBehaviour
{
    public GameObject skeletonPrefab;
    public int skeletonCount = 3;
    public float skeletonSpawnOffsetX = 1f;

    public void ExecuteAttack5()
    {
        for (int i = 0; i < skeletonCount; i++)
        {
            Vector3 spawnPos = transform.position + new Vector3((i - (skeletonCount - 1) / 2.0f) * skeletonSpawnOffsetX, 0, 0);
            Instantiate(skeletonPrefab, spawnPos, Quaternion.identity);
        }
    }
}

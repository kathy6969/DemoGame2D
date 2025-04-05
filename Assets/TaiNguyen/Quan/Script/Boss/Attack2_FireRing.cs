using UnityEngine;

public class Attack2_FireOrbs : MonoBehaviour
{
    public GameObject fireOrbPrefab;
    public int orbCount = 6;
    public float orbRadius = 2f;
    public Transform bossCenter; // Gán boss transform (hoặc vị trí trung tâm)

    // Gọi qua Animation Event
    public void ExecuteAttack2()
    {
        for (int i = 0; i < orbCount; i++)
        {
            float angle = i * Mathf.PI * 2f / orbCount;
            Vector3 spawnPos = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * orbRadius;
            GameObject orb = Instantiate(fireOrbPrefab, spawnPos, Quaternion.identity);

            // Gán center cho fire orb để nó có thể bay quanh boss
            FireOrbOrbit orbitScript = orb.GetComponent<FireOrbOrbit>();
            if (orbitScript != null && bossCenter != null)
            {
                orbitScript.center = bossCenter;
            }
        }
    }
}
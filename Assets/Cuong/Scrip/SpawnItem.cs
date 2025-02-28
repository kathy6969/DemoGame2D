using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{

    public List<GameObject> itemPrefabs; // Danh sách các loại item có thể spawn
    public Transform player; // Tham chiếu đến Player
    public float spawnDistance = 8f; // Khoảng cách spawn trước mặt player
    public float spawnInterval = 5f; // Chu kỳ spawn (5s)
    public float itemLifeTime = 10f; // Thời gian tồn tại của item

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnItemPeriodically());
    }
    IEnumerator SpawnItemPeriodically()
    {
        while (true) // Lặp vô hạn
        {
            SpawnItemInFrontOfPlayer();
            yield return new WaitForSeconds(spawnInterval); // Đợi 5s
        }
    }

    void SpawnItemInFrontOfPlayer()
    {
        if (itemPrefabs.Count == 0 || player == null) return;

        // Chọn 1 item ngẫu nhiên trong danh sách
        GameObject randomItem = itemPrefabs[Random.Range(0, itemPrefabs.Count)];

        // Xác định vị trí spawn trước mặt player
        Vector2 spawnPosition = (Vector2)player.position + (Vector2)player.right * spawnDistance;

        // Spawn item
        GameObject spawnedItem = Instantiate(randomItem, spawnPosition, Quaternion.identity);
        Destroy(spawnedItem, itemLifeTime); // Hủy item sau itemLifeTime giây
    }

}

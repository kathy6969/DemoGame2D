using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneTransitionOnCollision : MonoBehaviour
{
    // Danh sách các scene muốn chuyển tới
    private List<string> scenesList = new List<string> { "Map 1","Map 2", "Map 3", "Map 4", "Map 5", "Map 6" };
    private static int currentSceneIndex = 0;
    private HealthSystem healthSystem;
    void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Finish"))
        {
            // Chuyển sang scene tiếp theo trong danh sách
            LoadNextScene();
        }
        if (collider.gameObject.CompareTag("die"))
        {
            LoadCurrentScene();
        }
    }

    // Hàm chuyển sang scene tiếp theo trong danh sách
    public void LoadNextScene()
    {
        currentSceneIndex = (currentSceneIndex + 1) % scenesList.Count;

        // Chuyển đến scene tiếp theo trong danh sách
        Debug.Log("Loading scene: " + scenesList[currentSceneIndex]);
        SceneManager.LoadScene(scenesList[currentSceneIndex]);

        // Đảm bảo Player sẽ ở vị trí (0, 0, 0) sau khi chuyển scene
        StartCoroutine(WaitAndMovePlayer());
    }
    public void LoadCurrentScene()
    {
        healthSystem.TotalHealth = healthSystem.MaxHealth;
        
        StartCoroutine(WaitAndMovePlayer());
    }
    
    private IEnumerator WaitAndMovePlayer()
    {
        yield return null;
        transform.position = Vector3.zero;
    }
}
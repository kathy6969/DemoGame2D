using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneTransitionOnCollision : MonoBehaviour
{
    // Danh sách các scene muốn chuyển tới
    private List<string> scenesList = new List<string> { "Map 1","Map 2", "Map 3", "Map 4", "Map 5", "Map 6" };
    private int currentSceneIndex = 0;

    // Kiểm tra va chạm với đối tượng có tag "Finish"
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Kiểm tra nếu đối tượng va chạm có tag "Finish"
        if (collider.gameObject.CompareTag("Finish"))
        {
            // Chuyển sang scene tiếp theo trong danh sách
            LoadNextScene();
        }
        if (collider.gameObject.CompareTag("die"))
        {
            SceneManager.LoadScene("Map 1");
        }
    }

    // Hàm chuyển sang scene tiếp theo trong danh sách
    private void LoadNextScene()
    {
        // Tăng chỉ mục của scene và đảm bảo không vượt quá số lượng trong danh sách
        currentSceneIndex = (currentSceneIndex + 1) % scenesList.Count;

        // Chuyển đến scene tiếp theo trong danh sách
        Debug.Log("Loading scene: " + scenesList[currentSceneIndex]);
        SceneManager.LoadScene(scenesList[currentSceneIndex]);

        // Đảm bảo Player sẽ ở vị trí (0, 0, 0) sau khi chuyển scene
        StartCoroutine(WaitAndMovePlayer());
    }

    // Coroutine để đảm bảo Player được di chuyển sau khi scene đã tải
    private IEnumerator WaitAndMovePlayer()
    {
        // Đợi một khung hình để đảm bảo scene đã tải
        yield return null;

        // Đặt Player về vị trí (0, 0, 0)
        transform.position = Vector3.zero;
    }
}
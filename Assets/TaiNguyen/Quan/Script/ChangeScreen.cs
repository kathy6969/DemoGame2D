
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic; // Thêm nếu dùng TextMeshPro

public class ChangeScreen: MonoBehaviour
{
   
    // List chứa tên các scene đã thêm vào Build Settings
    [SerializeField] private List<string> sceneNames = new List<string>();

    /// <summary>
    /// Load scene dựa theo chỉ số trong list
    /// </summary>
    /// <param name="index">Chỉ số của scene trong list (bắt đầu từ 0)</param>
    public void LoadSceneByIndex(int index)
    {
        if (index >= 0 && index < sceneNames.Count)
        {
            SceneManager.LoadScene(sceneNames[index]);
        }
        else
        {
            Debug.LogWarning("Chỉ số scene không hợp lệ: " + index);
        }
    }

}

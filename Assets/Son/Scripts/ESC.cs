using UnityEngine;
using UnityEngine.SceneManagement;

public class ESC : MonoBehaviour
{
    public GameObject continueButton;
    public GameObject quitButton;

    private bool isPaused = false;

    void Start()
    {
        continueButton.SetActive(false);
        quitButton.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        if (isPaused == true)
        {
            continueButton.SetActive(false);
            quitButton.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    public void QuitGame()
    {
        Debug.Log("Thoát game...");
        Application.Quit();
        // Nếu chạy trong Unity Editor, dùng dòng sau:
        // UnityEditor.EditorApplication.isPlaying = false;
    }

    private void PauseGame()
    {
        continueButton.SetActive(true);
        quitButton.SetActive(true);
        //Time.timeScale = 0f;
        isPaused = true;
    }
}
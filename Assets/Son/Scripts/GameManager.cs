using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton

    public Text scoreText; // UI hiển thị điểm số
    private int score = 0; // Biến đếm chung

    private void Awake()
    {
        // Đảm bảo chỉ có một GameManager duy nhất
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void IncreaseScore(int amount)
    {
        score += amount; // Tăng điểm số
        scoreText.text = "Coin: " + score.ToString(); // Cập nhật UI
    }
}
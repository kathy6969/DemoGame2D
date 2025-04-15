using UnityEngine;
using UnityEngine.UI;

public class TextCoin : MonoBehaviour
{
    public static TextCoin Instance { get; private set; }

    public int totalCoins = 0;
    public Text coinText; // Dùng Text thay vì TextMeshProUGUI

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddCoin(int amount)
    {
        totalCoins += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + totalCoins;
        }
    }
}
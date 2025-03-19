using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DashManager : MonoBehaviour
{
    public static DashManager Instance { get; private set; }

    [Header("UI Settings")]
    public Slider dashSlider;   // Slider hiển thị tiến trình "sạc" dash
    public float fillTime = 3f; // Thời gian để slider đầy

    [Header("Dash Control")]
    public bool canDash = false;  // Biến kiểm tra nếu có thể dash

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (dashSlider != null)
        {
            dashSlider.value = 0;  // Bắt đầu từ giá trị 0
            StartCoroutine(FillSliderCoroutine());  // Bắt đầu tiến trình nạp slider
        }
        else
        {
            Debug.LogError("DashSlider is not assigned!");
        }
    }

    private IEnumerator FillSliderCoroutine()
    {
        float timer = 0f;
        // Tăng dần giá trị slider từ 0 đến 1 trong khoảng fillTime giây
        while (timer < fillTime)
        {
            timer += Time.deltaTime;
            dashSlider.value = timer / fillTime;
            yield return null;
        }

        dashSlider.value = 1;  // Khi slider đầy, đặt giá trị = 1
        canDash = true;  // Cho phép người chơi thực hiện dash
    }

    public void ResetDash()
    {
        // Reset slider về 0 khi dash được thực hiện
        dashSlider.value = 0;
        canDash = false;

        // Bắt đầu lại tiến trình nạp slider sau khi reset
        StartCoroutine(FillSliderCoroutine());
    }
}
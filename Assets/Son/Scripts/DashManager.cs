using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DashManager : MonoBehaviour
{
    // Singleton instance để truy cập từ bất kỳ đâu
    public static DashManager Instance { get; private set; }

    [Header("UI Settings")]
    public Slider dashSlider;   // Slider hiển thị tiến trình "sạc" dash
    public float fillTime = 3f; // Thời gian để slider đầy

    [Header("Dash Control")]
    // Biến kiểm tra dash: sẽ được set thành true khi slider đầy
    public bool canDash = false;

    private void Awake()
    {
        // Thiết lập Singleton: nếu đã có instance khác thì hủy đối tượng này
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
        // Đảm bảo slider khởi tạo từ 0
        if (dashSlider != null)
        {
            dashSlider.value = 0;
            StartCoroutine(FillSliderCoroutine());
        }
        else
        {
            Debug.LogError("Dash Slider chưa được gán trong Inspector!");
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
        
        dashSlider.value = 1;
        // Khi slider đầy, đặt canDash = true và không reset slider
        canDash = true;
        Debug.Log("Slider is full, canDash = true");
    }

    // Nếu sau khi dash được kích hoạt bạn muốn reset slider,
    // có thể gọi phương thức này từ một script khác:
    public void ResetDash()
    {
        dashSlider.value = 0;
        canDash = false;
        StartCoroutine(FillSliderCoroutine());
    }
}
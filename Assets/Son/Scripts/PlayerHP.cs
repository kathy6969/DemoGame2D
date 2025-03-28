using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    // Singleton
    public static PlayerHP Instance;

    // Máu của Player
    public float maxHP = 100f;
    private float currentHP;

    // Slider HP (nếu bạn sử dụng UI slider để hiển thị HP)
    public UnityEngine.UI.Slider hpSlider;

    void Awake()
    {
        // Kiểm tra xem đã có instance chưa, nếu chưa thì tạo mới
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Đặt máu ban đầu
        currentHP = 80f;
        UpdateSlider();
    }

    // Hàm truyền máu vào (ví dụ: từ Item)
    public void AddHealth(float amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);  // Đảm bảo không vượt quá maxHP
        UpdateSlider();
    }

    // Cập nhật Slider hiển thị HP
    private void UpdateSlider()
    {
        if (hpSlider != null)
        {
            hpSlider.value = currentHP / maxHP;
        }
    }

    // Hàm để lấy giá trị HP hiện tại
    public float GetCurrentHP()
    {
        return currentHP;
    }
}
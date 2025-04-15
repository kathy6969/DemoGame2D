using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float MaxHealth, TotalHealth;
    [SerializeField] FloatingHealbar Healbar;
    public float invincibleTime = 0.75f; // Thời gian bất tử sau khi nhận sát thương
    public bool isInvincible = false;  // Trạng thái bất tử
    private DebuffSystem debuffSystem;

    void Start()
    {
        Healbar = GetComponentInChildren<FloatingHealbar>(); 
        TotalHealth = MaxHealth;
        Healbar.UpdateHealbar(TotalHealth, MaxHealth);
        debuffSystem = GetComponent<DebuffSystem>();
    }

    public void DamageTake(float damage)
    {
        if (isInvincible) return; // Nếu đang bất tử thì bỏ qua sát thương

        TotalHealth -= damage;
        Healbar.UpdateHealbar(TotalHealth, MaxHealth);

        if (TotalHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(BecomeInvincible()); // Kích hoạt bất tử tạm thời
        }
    }

    private IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime); // Chờ hết thời gian bất tử
        isInvincible = false;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
    // 📌 Thêm hàm này để Fireball có thể gọi
    public void ApplyBurn(float burnPercentage, float duration)
    {
        if (debuffSystem != null)
        {
            debuffSystem.ApplyBurn(burnPercentage, duration);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float MaxHealth=100, TotalHealth;
    [SerializeField] FloatingHealbar Healbar;
    public float invincibleTime = 0.75f; // Thời gian bất tử sau khi nhận sát thương
    public bool isInvincible = false;  // Trạng thái bất tử
    private DebuffSystem debuffSystem;
    private SceneTransitionOnCollision sceneTransition;
    
    void Start()
    {
        //Debug.Log(transform.gameObject.tag);
        Healbar = GetComponentInChildren<FloatingHealbar>(); 
        TotalHealth = MaxHealth;
        Healbar.UpdateHealbar(TotalHealth, MaxHealth);
        debuffSystem = GetComponent<DebuffSystem>();
        sceneTransition = FindObjectOfType<SceneTransitionOnCollision>();
    }

    private void FixedUpdate()
    {
        Healbar.UpdateHealbar(TotalHealth, MaxHealth);
    }

    public void DamageTake(float damage)
    {
        if (isInvincible) return; // Nếu đang bất tử thì bỏ qua sát thương

        TotalHealth -= damage;
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
    if (transform.gameObject.tag == "Player")
    {
        sceneTransition.LoadCurrentScene();
    }
    else
    {
        // Nếu object này là boss, gọi CotManager
        if (gameObject.CompareTag("Enemy")) // 📌 Đảm bảo boss có tag "Boss"
        {
            CotManager cotManager = FindObjectOfType<CotManager>();
            if (cotManager != null)
            {
                cotManager.OnBossDefeated();
            }
        }
        Destroy(gameObject);
    }
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

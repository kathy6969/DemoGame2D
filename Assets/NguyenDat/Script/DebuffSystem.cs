using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffSystem : MonoBehaviour
{
    private HealthSystem healthSystem;
    private bool isBurning = false; // Kiểm tra có đang bị đốt không
    private Coroutine burnCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ApplyBurn(float burnPercentage, float duration)
    {
        if (isBurning)
            return; // Nếu đang cháy rồi thì không stack thêm

        isBurning = true;
        burnCoroutine = StartCoroutine(BurnEffect(burnPercentage, duration));
    }

    private IEnumerator BurnEffect(float burnPercentage, float duration)
    {
        float timer = 0;
        float burnDamage = healthSystem.MaxHealth * (burnPercentage / 100f); // Tính sát thương theo % máu tối đa

        while (timer < duration)
        {
            healthSystem.DamageTake(burnDamage); // Mất máu theo thời gian
            yield return new WaitForSeconds(1f);
            timer += 1f;
        }

        isBurning = false; // Hết hiệu ứng đốt
    }
}

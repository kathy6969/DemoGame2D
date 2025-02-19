using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float MaxHeal , TotalHeal;
    [SerializeField] FloatingHealbar Healbar;
    // Start is called before the first frame update
    void Start()
    {
        Healbar = GetComponentInChildren<FloatingHealbar>();
        TotalHeal = MaxHeal;
        Healbar.UpdateHealbar(TotalHeal, MaxHeal);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void DamageTake(float damage)
    {
        TotalHeal -= damage;
        Healbar.UpdateHealbar(TotalHeal,MaxHeal);
        if (TotalHeal < 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}

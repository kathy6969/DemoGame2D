﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetDamage(float ATKDAME)
    {
        damage = ATKDAME;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Lấy component HealthSystem của đối tượng bị va chạm
        HealthSystem targetHealth = collision.GetComponent<HealthSystem>();

        // Kiểm tra nếu đối tượng có HealthSystem
        if (targetHealth != null)
        {
            // Nếu object này có tag là "Enemy", nó chỉ gây damage cho Player
            if (gameObject.CompareTag("Enemy") && collision.CompareTag("Player"))
            {
                targetHealth.DamageTake(damage);
            }
            // Nếu object này có tag khác "Enemy" (ví dụ như "Player"), nó chỉ gây damage cho Enemy
            else if (!gameObject.CompareTag("Enemy") && collision.CompareTag("Enemy"))
            {
                targetHealth.DamageTake(damage);
            }
        }
    }

}

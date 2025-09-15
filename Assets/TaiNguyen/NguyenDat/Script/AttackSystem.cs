using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    public float damage;
    public bool isEnemyAttack = false;
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
        if (isEnemyAttack && collision.CompareTag("Player"))
        {
            HealthSystem playerHealth = collision.GetComponent<HealthSystem>();
            if (playerHealth != null)
            {
                playerHealth.DamageTake(damage);
            }
        }
        else if (!isEnemyAttack && collision.CompareTag("Enemy"))
        {
            HealthSystem enemyHealth = collision.GetComponent<HealthSystem>();
            if (enemyHealth != null)
            {
                enemyHealth.DamageTake(damage);
            }
        }
    }

}

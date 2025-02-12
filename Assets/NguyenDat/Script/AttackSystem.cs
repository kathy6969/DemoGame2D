using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    public int Damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthSystem targetHealth = collision.GetComponent<HealthSystem>();
        if (targetHealth != null)
        {
            targetHealth.DamageTake(-Damage);
        }
    }
}

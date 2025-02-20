using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    private BossStateMachine boss;
    private Transform player;
    private GameObject fireballPrefab;
    private int fireballDamage; // Thêm biến sát thương
    private Animator animator;
    public AttackState(BossStateMachine boss, GameObject fireballPrefab, Transform player, int fireballDamage)
    {
        this.boss = boss;
        this.fireballPrefab = fireballPrefab;
        this.player = player;
        this.fireballDamage = fireballDamage; // Nhận sát thương từ boss
        animator = boss.GetComponent<Animator>();
    }
    public override void EnterState()
    {
        Debug.Log("Boss vào trạng thái tấn công!");
        
        animator.SetBool("Attack", true);
    }
    public override void UpdateState()
    {
        FlipTowardsPlayer();
    }
    public override void ExitState()
    {
        animator.SetBool("Attack", false);
    }
    private void FlipTowardsPlayer()
    {
        if (player != null)
        {
            float direction = player.position.x - boss.transform.position.x;
            float scaleX = Mathf.Abs(boss.transform.localScale.x); // Lấy giá trị tuyệt đối để giữ nguyên tỷ lệ gốc

            if (direction > 0)
            {
                boss.transform.localScale = new Vector3(scaleX, boss.transform.localScale.y, boss.transform.localScale.z);
            }
            else
            {
                boss.transform.localScale = new Vector3(-scaleX, boss.transform.localScale.y, boss.transform.localScale.z);
            }
        }
    }
}

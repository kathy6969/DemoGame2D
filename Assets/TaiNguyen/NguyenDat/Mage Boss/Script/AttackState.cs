using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackState : State
{
    private BossStateMachine boss;
    private Transform player;
    private GameObject fireballPrefab;
    private int fireballDamage; // Thêm biến sát thương
    private AdvFireBall fireballScript;
    private Transform FirePoint;
    private Animator animator;
    private float switchTime = 2f; // Thời gian đổi animation
    private bool isAttack1 = true;
    private MonoBehaviour coroutineRunner;
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
        isAttack1 = Random.value > 0.5f;
        PlayAttackAnimation();
        // Lấy reference đến một MonoBehaviour (BossStateMachine chẳng hạn)
        coroutineRunner = animator.GetComponent<MonoBehaviour>();

        if (coroutineRunner != null)
        {
            coroutineRunner.StartCoroutine(SwitchAttackAnimationCoroutine());
        }
    }

    public override void UpdateState()
    {
        FlipTowardsPlayer();
    }

    public override void ExitState()
    {
        if (coroutineRunner != null)
        {
            coroutineRunner.StopCoroutine(SwitchAttackAnimationCoroutine());
        }
    }

    private void PlayAttackAnimation()
    {
        string[] attackTriggers = { "Fireball", "Firerain", "Fireburst" };
        string triggerName = attackTriggers[Random.Range(0, attackTriggers.Length)];
        animator.SetTrigger(triggerName);
    }

    private IEnumerator SwitchAttackAnimationCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(switchTime);
            SwitchAttackAnimation();
        }
    }

    private void SwitchAttackAnimation()
    {
        isAttack1 = !isAttack1;
        PlayAttackAnimation();
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

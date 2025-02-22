using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdelState : State
{
    private BossStateMachine boss;
    private Animator animator;
    public IdelState(BossStateMachine boss)
    {
        this.boss = boss;
        animator = boss.GetComponent<Animator>();
    }
    public override void EnterState()
    {
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
    }

    public override void ExitState()
    {
        
    }
}

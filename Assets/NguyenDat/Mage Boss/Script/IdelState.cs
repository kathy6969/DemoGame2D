using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdelState : State
{
    private BossStateMachine boss;
    private Animator animator;
    private BossStateMachine bossStateMachine;

    public IdelState(BossStateMachine bossStateMachine)
    {
        this.bossStateMachine = bossStateMachine;
    }

    public override void EnterState()
    {
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
    }

    public override void ExitState()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

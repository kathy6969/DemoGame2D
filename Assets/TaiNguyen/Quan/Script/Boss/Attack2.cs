using UnityEngine;

public class Attack2 : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PerformAttack()
    {
        animator.SetTrigger("Attack2");
    }
}

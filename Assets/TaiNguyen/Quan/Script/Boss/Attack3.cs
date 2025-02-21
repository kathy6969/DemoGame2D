using UnityEngine;

public class Attack3 : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PerformAttack()
    {
        animator.SetTrigger("Attack3");
    }
}

using System.Collections;
using UnityEngine;

public class ControllBoss : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 15f;
    public float attackCooldown = 10f;

    private MoveBoss moveBoss;
    private Attack1 attack1;
    private Attack2 attack2;
    private Attack3 attack3;
    private Attack4 attack4;
    private Animator animator;
    private bool canAttack = true;

    void Start()
    {
        moveBoss = GetComponent<MoveBoss>();
        attack1 = GetComponent<Attack1>();
        attack2 = GetComponent<Attack2>();
        attack3 = GetComponent<Attack3>();
        attack4 = GetComponent<Attack4>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectionRange && canAttack)
        {
            StartCoroutine(PerformAttack());
        }
    }

    IEnumerator PerformAttack()
    {
        canAttack = false;

        int attackType = ChooseAttack();
        switch (attackType)
        {
            case 1:
                if (Vector2.Distance(transform.position, player.position) > 3f)
                    yield return StartCoroutine(MoveToPlayer());
                attack1.PerformAttack();
                break;
            case 2:
                if (Vector2.Distance(transform.position, player.position) > 3f)
                    yield return StartCoroutine(MoveToPlayer());
                attack2.PerformAttack();
                break;
            case 3:
                attack3.PerformAttack();
                break;
            case 4:
                attack4.PerformAttack();
                break;
        }

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator MoveToPlayer()
    {
        while (Vector2.Distance(transform.position, player.position) > 3f)
        {
            float moveDir = (player.position.x > transform.position.x) ? 1 : -1;
            transform.Translate(Vector2.right * moveDir * moveBoss.speed * Time.deltaTime);
            yield return null;
        }
    }

    int ChooseAttack()
    {
        int randomValue = Random.Range(1, 101);
        if (randomValue <= 35) return 1;
        if (randomValue <= 70) return 2;
        if (randomValue <= 80) return 3;
        return 4;
    }
}

using System.Collections;
using UnityEngine;

public class MoveBoss : MonoBehaviour
{
    public float speed = 2f;
    public float walkDistance = 10f;
    public float idleTime = 5f;

    private Vector2 startPos;
    private bool movingRight = true;
    private bool isIdle = false;
    private Animator animator;

    void Start()
    {
        startPos = transform.position;
        animator = GetComponent<Animator>();
        StartCoroutine(MovePattern());
    }

    void Update()
    {
        if (isIdle) return;

        float moveDir = movingRight ? 1 : -1;
        transform.Translate(Vector2.right * moveDir * speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - startPos.x) >= walkDistance)
        {
            movingRight = !movingRight;
            Flip();
        }
    }

    IEnumerator MovePattern()
    {
        while (true)
        {
            yield return new WaitForSeconds(idleTime);
            isIdle = true;
            animator.SetTrigger("Idle");
            yield return new WaitForSeconds(idleTime);
            isIdle = false;
            animator.SetTrigger("Walk");
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}

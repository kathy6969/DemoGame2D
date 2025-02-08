using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float distance = 5f;
    private Vector3 startPos;
    private bool WalkRight = true;
    public float timer;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float leftBound = startPos.x - distance;
        float rightBound = startPos.x + distance;
        if (WalkRight) {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBound) { 
                WalkRight = false;
                Flip();
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBound)
            {
                WalkRight = true;
                Flip();
            }
        }

        timer += Time.deltaTime;
        if (timer > 2f) //dk hon 3s tan cong 1 lan
        {
            animator.SetTrigger("IsAttack");
            timer = 0.0f;
        }
    }
    void Flip() { 
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}

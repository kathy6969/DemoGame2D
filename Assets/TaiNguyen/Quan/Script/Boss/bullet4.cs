using UnityEngine;

public class FireOrb : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 moveDirection;

    public void Launch(Vector2 direction)
    {
        moveDirection = direction.normalized;

        // Lật sprite nếu bắn sang trái
        if (direction.x < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}

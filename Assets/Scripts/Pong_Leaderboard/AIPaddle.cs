using UnityEngine;

public class AIPaddle : MonoBehaviour
{
    public float speed = 10.0f;
    public Transform ball;
    private Rigidbody2D rb;
    public float errorMargin = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Vector2.Distance(ball.position, transform.position) > 8.5f)
            return;

        float error = Random.Range(-errorMargin, errorMargin);
        Vector2 targetPosition = new Vector2(transform.position.x, ball.position.y + error);
        Vector2 newPosition = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        newPosition.y = Mathf.Clamp(newPosition.y, -2.9f, 2.9f);
        rb.MovePosition(newPosition);
    }
}

using UnityEngine;

public class PlayerPaddle : MonoBehaviour
{
    public float speed = 10.0f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float move = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(0, move * speed);

        // Clamp the paddle's Y position
        Vector2 clampedPosition = transform.position;
        clampedPosition.y = Mathf.Clamp(transform.position.y, -2.9f, 2.9f);
        transform.position = clampedPosition;
    }
}

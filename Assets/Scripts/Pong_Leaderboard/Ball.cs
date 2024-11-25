using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody2D rb;
    private AudioSource hitSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hitSound = GetComponent<AudioSource>();
        LaunchBall();
    }

    void LaunchBall()
    {
        transform.position = Vector3.zero;
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(-0.5f, 0.5f);
        Vector2 direction = new Vector2(x, y).normalized;
        rb.velocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hitSound != null)
        {
            hitSound.Play();
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            wayBall(collision, 1);
        }
        if (collision.gameObject.CompareTag("AI"))
        {
            wayBall(collision, -1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerZone"))
        {
            GameManager.Instance.AddScore("AI");
            StartCoroutine(RestartBall(-1f));
        }
        if (collision.gameObject.CompareTag("AIZone"))
        {
            GameManager.Instance.AddScore("Player");
            StartCoroutine(RestartBall(1f));
        }
    }

    IEnumerator RestartBall(float direction)
    {
        yield return new WaitForSeconds(1);
        transform.position = Vector3.zero;
        rb.velocity = new Vector2(direction, Random.Range(-0.5f, 0.5f)).normalized * speed;
    }

    private void wayBall(Collision2D collision, int x)
    {
        float hitPoint = (transform.position.y - collision.transform.position.y) / collision.collider.bounds.size.y;
        float angle = hitPoint * 60f; // Maximum 60-degree angle
        Vector2 direction = new Vector2(x, hitPoint).normalized;
        rb.velocity = direction * speed;
    }
}
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public BatBehaviour player1;
    public BatBehaviour player2;
    public float size = 1.0f;
    float speed = 5f;

    Vector2 direction;
    bool isPlaying = true;

    void Start()
    {
        direction = new Vector2(1f, 1f).normalized;
    }

    void Update()
    {
        if (isPlaying)
            transform.localPosition += (Vector3)(direction * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isPlaying) return;

        switch (collision.gameObject.name)
        {
            case "Left Bat":
            case "Right Bat":
                direction.y = Mathf.Abs(direction.y);
                speed += 0.5f;
                break;
            case "Left Goal":
                player2.IncreaseScore();
                isPlaying = false;
                break;
            case "Right Goal":
                player1.IncreaseScore();
                isPlaying = false;
                break;
            default:
                direction = Vector2.Reflect(direction, collision.GetContact(0).normal).normalized;
                break;
        }
    }
}
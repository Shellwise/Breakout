using UnityEngine;

public class BallBehaviour : MonoBehaviour
{   
    public BatBehaviour player1;
    public BatBehaviour player2;
    public float size = 1.0f;
    float speed = 5f;
    
    Vector3 direction;
    bool isPlaying = true;

    void Start()
    {
        direction = Vector3.right;
    }
    void Update()
    {
        if (isPlaying)
        {
        transform.localPosition += direction * speed * Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
            if (isPlaying)
            {
                switch (collision.gameObject.name)
            {
            case "Left Bat":
                ChangeDirection();
                break;
            case "Right Bat":
                ChangeDirection();
                break;
            case "Left Goal":
               player2.IncreaseScore();
               isPlaying = false;
                break;
            case "Right Goal":
               player1.IncreaseScore();
               isPlaying = false;
                break;
            }
        }   
    }

    void ChangeDirection()
    {
        if (direction == Vector3.right)
            direction = Vector3.left;
        else
            direction = Vector3.right;

        speed += 0.5f;
    }
}
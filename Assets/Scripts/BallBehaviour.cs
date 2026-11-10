using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public BatBehaviour player1;
    public BatBehaviour player2;
    public float size = 1.0f;
    float speed = 5f;

    [Header("Power Up Settings")]
    public float powerUpDuration = 5f;
    public float minSpeed = 2f;
    public float maxSpeed = 12f;

    [Header("Debug")]
    public bool logSpeedChanges = true;

    float speedMultiplier = 1f;
    float effectTimer = 0f;

    Vector2 direction;
    bool isPlaying = true;

    void Start()
    {
        direction = new Vector2(1f, 1f).normalized;
    }

    void Update()
    {
        // Count down the active speed effect
        if (effectTimer > 0f)
        {
            effectTimer -= Time.deltaTime;

            if (effectTimer <= 0f)
            {
                speedMultiplier = 1f;
                effectTimer = 0f;

                if (logSpeedChanges)
                    Debug.Log("Ball speed back to normal: " + GetCurrentSpeed());
            }
        }

        if (isPlaying)
            transform.localPosition += (Vector3)(direction * GetCurrentSpeed() * Time.deltaTime);
    }

    float GetCurrentSpeed()
    {
        return Mathf.Clamp(speed * speedMultiplier, minSpeed, maxSpeed);
    }

    public void ChangeSpeed(float multiplier)
    {
        // Replaces any active speed effect rather than stacking
        speedMultiplier = multiplier;
        effectTimer = powerUpDuration;

        if (logSpeedChanges)
            Debug.Log("Ball speed changed to " + GetCurrentSpeed() +
                      " for " + powerUpDuration + "s");
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
using UnityEngine;

// Put this on your Bottom Wall object.
public class BottomWall : MonoBehaviour
{
    public GameStateManager gameState;   // drag your GameManager here

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<BallBehaviour>() != null)
            gameState.Lose();
    }

    // Also works if you tick "Is Trigger" on the wall's collider
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BallBehaviour>() != null)
            gameState.Lose();
    }
}
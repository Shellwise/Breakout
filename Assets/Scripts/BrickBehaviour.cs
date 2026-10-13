using UnityEngine;

public class BrickBehaviour : MonoBehaviour
{
    public bool isBreakable;
    public BrickSpawner spawner;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball"))
            return;

        if (isBreakable)
        {
            spawner.BrickDestroyed();
            Destroy(gameObject);
        }
    }
}
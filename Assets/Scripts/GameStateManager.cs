using UnityEngine;
using UnityEngine.UI;

// Put this on your GameManager object.
// Retry and Continue reset the level in place, without reloading the scene,
// so the main menu never comes back and the music keeps playing.
public class GameStateManager : MonoBehaviour
{
    [Header("Ball")]
    public Rigidbody2D ball;            // drag your Ball here

    [Header("UI panels (start them switched OFF)")]
    public GameObject losePanel;        // holds "Game Over" text + Retry button
    public GameObject winPanel;         // holds "Level Clear" text + Continue button

    [Header("Bricks (found automatically if left empty)")]
    public BrickSpawner brickSpawner;

    Vector3 ballStartPos;
    bool roundOver;
    float lastSpeed;

    void Start()
    {
        ballStartPos = ball.transform.position;
        if (brickSpawner == null) brickSpawner = FindFirstObjectByType<BrickSpawner>();

        if (losePanel) losePanel.SetActive(false);
        if (winPanel) winPanel.SetActive(false);

        // Hook the buttons up here (and clear any Inspector links so they don't fire twice)
        HookButton(losePanel, Retry);
        HookButton(winPanel, Continue);
    }

    void HookButton(GameObject panel, UnityEngine.Events.UnityAction action)
    {
        if (!panel) return;
        Button button = panel.GetComponentInChildren<Button>(true);
        if (!button) return;
        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(action);
    }

    void ResetBall()
    {
        ball.transform.position = ballStartPos;
        float speed = lastSpeed > 0.1f ? lastSpeed : 5f;
        Vector2 dir = new Vector2(Random.Range(-0.5f, 0.5f), 1f).normalized;
        ball.linearVelocity = dir * speed;
    }

    // Called by BottomWall when the ball hits it
    public void Lose()
    {
        if (roundOver) return;
        roundOver = true;
        lastSpeed = ball.linearVelocity.magnitude;
        Time.timeScale = 0f;            // freeze the game
        losePanel.SetActive(true);
    }

    // Called by BrickSpawner when the last breakable brick is broken
    public void Win()
    {
        if (roundOver) return;
        roundOver = true;
        lastSpeed = ball.linearVelocity.magnitude;
        Time.timeScale = 0f;
        winPanel.SetActive(true);
    }

    public void Retry()
    {
        losePanel.SetActive(false);
        if (brickSpawner) brickSpawner.ResetGrid();     // brand new random bricks
        ResetBall();
        if (ScoreManager.Instance != null) ScoreManager.Instance.ResetScore();

        roundOver = false;
        Time.timeScale = 1f;
    }

    public void Continue()
    {
        winPanel.SetActive(false);
        if (brickSpawner) brickSpawner.ResetGrid();     // brand new random bricks
        ResetBall();                                    // ball back to its spawn point

        roundOver = false;
        Time.timeScale = 1f;
    }
}
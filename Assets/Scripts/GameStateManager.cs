using System.Collections.Generic;
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

    [Header("Bricks")]
    public string brickTag = "Brick";   // tag ONLY your breakable bricks with this

    // A saved copy of every brick, so the level can be rebuilt
    struct SavedBrick
    {
        public GameObject copy;
        public Transform parent;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }
    List<SavedBrick> savedBricks = new List<SavedBrick>();
    GameObject brickStorage;

    Vector3 ballStartPos;
    bool roundOver;
    bool bricksHaveSpawned;
    float lastSpeed;

    void Start()
    {
        ballStartPos = ball.transform.position;
        if (losePanel) losePanel.SetActive(false);
        if (winPanel) winPanel.SetActive(false);

        // Hook the buttons up here (and clear any Inspector links so they don't fire twice)
        HookButton(losePanel, Retry);
        HookButton(winPanel, Continue);

        InvokeRepeating(nameof(CheckBricks), 0.5f, 0.25f);
    }

    void HookButton(GameObject panel, UnityEngine.Events.UnityAction action)
    {
        if (!panel) return;
        Button button = panel.GetComponentInChildren<Button>(true);
        if (!button) return;
        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(action);
    }

    void CheckBricks()
    {
        if (roundOver) return;

        GameObject[] bricks = GameObject.FindGameObjectsWithTag(brickTag);

        if (bricks.Length > 0)
        {
            // First time we see the bricks, save a copy of the whole layout
            if (!bricksHaveSpawned && savedBricks.Count == 0) SaveBricks(bricks);
            bricksHaveSpawned = true;
            return;
        }

        if (bricksHaveSpawned) Win();
    }

    void SaveBricks(GameObject[] bricks)
    {
        brickStorage = new GameObject("Saved Bricks (hidden)");
        brickStorage.SetActive(false);

        foreach (GameObject brick in bricks)
        {
            SavedBrick saved = new SavedBrick
            {
                copy = Instantiate(brick, brickStorage.transform),
                parent = brick.transform.parent,
                position = brick.transform.position,
                rotation = brick.transform.rotation,
                scale = brick.transform.localScale
            };
            savedBricks.Add(saved);
        }
    }

    void RebuildBricks()
    {
        // Remove whatever bricks are left
        foreach (GameObject brick in GameObject.FindGameObjectsWithTag(brickTag))
            Destroy(brick);

        // Put a fresh copy of every saved brick back
        foreach (SavedBrick saved in savedBricks)
        {
            GameObject brick = Instantiate(saved.copy, saved.parent);
            brick.name = saved.copy.name.Replace("(Clone)", "");
            brick.transform.position = saved.position;
            brick.transform.rotation = saved.rotation;
            brick.transform.localScale = saved.scale;
        }
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

    void Win()
    {
        roundOver = true;
        lastSpeed = ball.linearVelocity.magnitude;
        Time.timeScale = 0f;
        winPanel.SetActive(true);
    }

    public void Retry()
    {
        losePanel.SetActive(false);
        RebuildBricks();
        ResetBall();

        // TODO: reset the score here once I've seen ScoreManager.cs

        bricksHaveSpawned = false;
        roundOver = false;
        Time.timeScale = 1f;
    }

    public void Continue()
    {
        winPanel.SetActive(false);
        RebuildBricks();                // fresh set of bricks
        ResetBall();                    // ball back to its spawn point

        bricksHaveSpawned = false;
        roundOver = false;
        Time.timeScale = 1f;
    }
}
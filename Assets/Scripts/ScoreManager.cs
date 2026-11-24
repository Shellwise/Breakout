using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI scoreText;
    public string prefix = "Score: ";

    int score = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateText();
    }

    public void AddPoint(int amount = 1)
    {
        score += amount;
        UpdateText();
    }

    public int GetScore()
    {
        return score;
    }

    // Called by GameStateManager when you press Retry
    public void ResetScore()
    {
        score = 0;
        UpdateText();
    }

    void UpdateText()
    {
        if (scoreText != null)
            scoreText.text = prefix + score;
    }
}
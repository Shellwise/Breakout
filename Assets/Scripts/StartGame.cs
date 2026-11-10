using UnityEngine;

public class StartGame : MonoBehaviour
{
    [Tooltip("Objects that make up the start screen (hidden when the game starts)")]
    public GameObject[] objectsToHide;

    [Tooltip("Objects that only appear once the game starts, like the score text")]
    public GameObject[] objectsToShow;

    void Start()
    {
        Time.timeScale = 0f;

        // Hide the in-game objects until the player presses start
        foreach (GameObject obj in objectsToShow)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

    public void StartPlaying()
    {
        Time.timeScale = 1f;

        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        foreach (GameObject obj in objectsToShow)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }
}
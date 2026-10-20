
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameObject canvas;

    void Start()
    {
        Time.timeScale = 0f;
    }

    public void StartPlaying()
    {
        Time.timeScale = 1f;
        canvas.SetActive(false);
    }
}

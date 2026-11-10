using UnityEngine;
using TMPro;
using System.Collections;

public class BatBehaviour : MonoBehaviour
{
    float speed = 5f;
    int score = 0;
    public TextMeshProUGUI scoreText;

    [Header("Power Up Settings")]
    public float minWidth = 0.5f;
    public float maxWidth = 6f;
    public float powerUpDuration = 5f;

    Vector3 baseScale;
    Coroutine resetRoutine;

    void Awake()
    {
        baseScale = transform.localScale;
    }

    void Start()
    {

    }

    void Update()
    {
        if (gameObject.name == "MainBat")
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = new Vector3(worldPosition.x, transform.position.y, transform.position.z);
        }
    }

    public void ChangeWidth(float multiplier)
    {
        // Always calculate from the original size so effects don't stack
        Vector3 s = baseScale;
        s.x = Mathf.Clamp(baseScale.x * multiplier, minWidth, maxWidth);
        transform.localScale = s;

        if (resetRoutine != null)
            StopCoroutine(resetRoutine);
        resetRoutine = StartCoroutine(ResetWidthAfterDelay());
    }

    IEnumerator ResetWidthAfterDelay()
    {
        yield return new WaitForSeconds(powerUpDuration);
        transform.localScale = baseScale;
        resetRoutine = null;
    }

    public void IncreaseScore()
    {
    }

    public int GetScore()
    {
        return score;
    }
}
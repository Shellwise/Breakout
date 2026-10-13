using UnityEngine;
using TMPro;

public class BatBehaviour : MonoBehaviour
{
    float speed = 5f;
    int score = 0;
    public TextMeshProUGUI scoreText;




    // Start is called once before the first execution of Update after the MonoBehaviour is created 
    void Start()
    {

    }



    // Update is called once per frame
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

    public void IncreaseScore()
    {
    }

    public int GetScore()
    {
        return score;
    }










}
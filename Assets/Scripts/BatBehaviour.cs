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
        if  (Input.GetKey(KeyCode.UpArrow) && gameObject.name == "Right Bat"|| (Input.GetKey(KeyCode.W) && gameObject.name == "Left Bat"))
        {
            transform.localPosition += Vector3.up * speed * Time.deltaTime;
         }
        else if ((Input.GetKey(KeyCode.DownArrow) && gameObject.name =="Right Bat") || (Input.GetKey(KeyCode.S) && gameObject.name == "Left Bat")) 
        {      
            transform.localPosition += Vector3.down * speed * Time.deltaTime;
        }
    }

    public void IncreaseScore()
    {
        score++;
        // string message = gameObject.name == "Left Bat" ? "Player 1 - " + score :"Player 2 - " + score;
        // Debug.Log(message);

    //     scoreText.text = gameObject.name == "Left Bat" ? "Player 1 -" + score : "Player 2 -" + score;
    }

    public int GetScore()
    {
        return score;
    }










}
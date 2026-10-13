using UnityEngine;

public class BallSpawnerBehaviour : MonoBehaviour
{
    public BallBehaviour ball;
    BallBehaviour[] ballClones;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ballClones = new BallBehaviour[5];
        SpawnBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SpawnBall();
        } else if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < ballClones.Length; i++)
            {
                if (ballClones[i] != null); 
                     Destroy(ballClones[i].gameObject);
            }

            SpawnBall();
        }

    }

   void SpawnBall()
    {
        for (int i = 0; i < ballClones.Length; i++)
        {
            if (ballClones[i] == null)
        {
        ballClones[i] = Instantiate(ball);

        ballClones[i].transform.position = transform.position;

        ballClones[i].gameObject.SetActive(true);
    
            }
        }
    }
}

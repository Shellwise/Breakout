
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrickSpawner : MonoBehaviour
{
    public GameObject breakablePrefab;
    public GameObject unbreakablePrefab;

    public Vector2 startPosition = new Vector2(-8f, 3f);
    public float spacingX = 2f;
    public float spacingY = 1f;

    List<GameObject> bricks = new List<GameObject>();

    int remainingBreakable;
    bool respawning;

    void Start()
    {
        SpawnGrid();
    }

    void SpawnGrid()
    {
        bricks.Clear();
        respawning = false;

        List<Vector2> positions = new List<Vector2>();

        // 4 rows up and 5 bricks sideways
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                positions.Add(new Vector2(col, -row));
            }
        }

        // Randomly choose 2 to 4 unbreakable bricks
        int unbreakableCount = Random.Range(2, 5);
        remainingBreakable = positions.Count - unbreakableCount;

        // Shuffle brick positions
        for (int i = 0; i < positions.Count; i++)
        {
            int randomIndex = Random.Range(i, positions.Count);

            Vector2 temp = positions[i];
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }

        // Spawn all bricks
        for (int i = 0; i < positions.Count; i++)
        {
            SpawnBrick(positions[i], i >= unbreakableCount);
        }
    }

    void SpawnBrick(Vector2 gridPosition, bool breakable)
    {
        Vector2 pos = startPosition + new Vector2(
            gridPosition.x * spacingX,
            gridPosition.y * spacingY
        );

        GameObject prefab = breakable ? breakablePrefab : unbreakablePrefab;

        GameObject brick = Instantiate(prefab, pos, Quaternion.identity);
        brick.SetActive(true);

        SpriteRenderer[] renderers =
            brick.GetComponentsInChildren<SpriteRenderer>(true);

        foreach (SpriteRenderer sr in renderers)
        {
            sr.enabled = true;
            sr.color = breakable ? Color.white : Color.red;
            sr.sortingOrder = 10;
        }

        BrickBehaviour behaviour = brick.GetComponent<BrickBehaviour>();

        if (behaviour == null)
        {
            Debug.LogError("Add BrickBehaviour to both brick prefabs!");
            Destroy(brick);
            return;
        }

        behaviour.isBreakable = breakable;
        behaviour.spawner = this;

        bricks.Add(brick);
    }

    public void BrickDestroyed()
    {
        if (respawning)
            return;

        remainingBreakable--;

        if (remainingBreakable <= 0)
        {
            respawning = true;
            StartCoroutine(RespawnGrid());
        }
    }

    IEnumerator RespawnGrid()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (GameObject brick in bricks)
        {
            if (brick != null)
            {
                Destroy(brick);
            }
        }

        bricks.Clear();
        SpawnGrid();
    }
}

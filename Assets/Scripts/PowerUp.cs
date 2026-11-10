using UnityEngine;

public enum PowerUpType { WiderBat, SlimmerBat, FasterBall, SlowerBall }

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public float fallSpeed = 3f;

    [Header("Size")]
    [Tooltip("1 = same width as the ball, 1.5 = 50% bigger, etc.")]
    public float sizeRelativeToBall = 1f;

    [Header("Icons")]
    public Sprite widerBatIcon;
    public Sprite slimmerBatIcon;
    public Sprite fasterBallIcon;
    public Sprite slowerBallIcon;

    SpriteRenderer sr;
    BatBehaviour bat;
    BallBehaviour ball;
    Collider2D batCollider;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // Find the player's bat (the one named MainBat)
        foreach (BatBehaviour b in FindObjectsByType<BatBehaviour>(FindObjectsSortMode.None))
        {
            if (b.gameObject.name == "MainBat")
            {
                bat = b;
                batCollider = b.GetComponent<Collider2D>();
                break;
            }
        }

        ball = FindFirstObjectByType<BallBehaviour>();

        // Pick the icon for this type
        if (sr != null)
        {
            Sprite icon = null;
            switch (type)
            {
                case PowerUpType.WiderBat:   icon = widerBatIcon;   break;
                case PowerUpType.SlimmerBat: icon = slimmerBatIcon; break;
                case PowerUpType.FasterBall: icon = fasterBallIcon; break;
                case PowerUpType.SlowerBall: icon = slowerBallIcon; break;
            }

            if (icon != null)
                sr.sprite = icon;
        }

        MatchBallSize();
    }

    void MatchBallSize()
    {
        if (ball == null || sr == null || sr.sprite == null)
            return;

        // Work out the ball's width in world units
        float ballWidth;
        SpriteRenderer ballSr = ball.GetComponent<SpriteRenderer>();
        Collider2D ballCol = ball.GetComponent<Collider2D>();

        if (ballSr != null)
            ballWidth = ballSr.bounds.size.x;
        else if (ballCol != null)
            ballWidth = ballCol.bounds.size.x;
        else
            ballWidth = ball.transform.lossyScale.x;

        // Scale so this icon's width matches the ball's, regardless of source image size
        float spriteWidth = sr.sprite.bounds.size.x;
        if (spriteWidth <= 0f) return;

        float scale = (ballWidth * sizeRelativeToBall) / spriteWidth;
        transform.localScale = new Vector3(scale, scale, 1f);
    }

    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (transform.position.y < -15f)
        {
            Destroy(gameObject);
            return;
        }

        if (bat != null && TouchingBat())
        {
            Apply();
            Destroy(gameObject);
        }
    }

    bool TouchingBat()
    {
        Bounds batBounds = batCollider != null
            ? batCollider.bounds
            : bat.GetComponent<Renderer>().bounds;

        Bounds myBounds = sr != null
            ? sr.bounds
            : new Bounds(transform.position, Vector3.one * 0.5f);

        // Ignore the Z axis so the overlap check is purely 2D
        batBounds.extents = new Vector3(batBounds.extents.x, batBounds.extents.y, 100f);
        myBounds.extents = new Vector3(myBounds.extents.x, myBounds.extents.y, 100f);

        return batBounds.Intersects(myBounds);
    }

    void Apply()
    {
        switch (type)
        {
            case PowerUpType.WiderBat:
                bat.ChangeWidth(1.5f);
                break;
            case PowerUpType.SlimmerBat:
                bat.ChangeWidth(0.6f);
                break;
            case PowerUpType.FasterBall:
                if (ball != null) ball.ChangeSpeed(1.3f);
                break;
            case PowerUpType.SlowerBall:
                if (ball != null) ball.ChangeSpeed(0.7f);
                break;
        }
    }
}
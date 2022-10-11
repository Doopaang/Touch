using UnityEngine;

public class Cut : MonoBehaviour
{
    private SpriteRenderer renderer;

    [SerializeField]
    private Direction direction;
    private Vector3 direct;
    [SerializeField]
    private float speed;
    private Vector3 position;

    private bool start = false;
    [HideInInspector]
    public bool wait = false;

    private const float DELAY = 1.5f;

    public enum Direction
    {
        Up, Right, Down, Left, Center
    }

    void Start()
    {
        position = transform.position;
        Vector3 waitPos = transform.position;
        renderer = GetComponent<SpriteRenderer>();
        Vector2 size = renderer.sprite.rect.size;
        float height = 2 * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;

        switch (direction)
        {
            case Direction.Up:
                waitPos.y = -(height + size.y) * 0.5f;
                direct = Vector3.up;
                break;

            case Direction.Right:
                waitPos.x = -(width + size.x) * 0.5f;
                direct = Vector3.right;
                break;

            case Direction.Down:
                waitPos.y = (height + size.y) * 0.5f;
                direct = Vector3.down;
                break;

            case Direction.Left:
                waitPos.x = (width + size.x) * 0.5f;
                direct = Vector3.left;
                break;

            case Direction.Center:
                renderer.enabled = false;
                direct = Vector3.zero;
                break;
        }
        transform.position = waitPos;
    }

    void Update()
    {
        if (start)
        {
            transform.Translate(direct * speed * Time.deltaTime);

            if (CheckGoal())
            {
                transform.position = position;
                wait = false;
                start = false;
            }
        }
    }

    public void Action()
    {
        Invoke("ActionDelay", DELAY);
        wait = true;
    }

    public void ActionDelay()
    {
        start = true;
    }

    public void Skip()
    {
        transform.position = position;

        CancelInvoke("ActionDelay");
        wait = false;
        start = false;
    }

    private bool CheckGoal()
    {
        switch (direction)
        {
            case Direction.Up:
                return transform.position.y > position.y;

            case Direction.Right:
                return transform.position.x > position.x;

            case Direction.Down:
                return transform.position.y < position.y;

            case Direction.Left:
                return transform.position.x < position.x;

            case Direction.Center:
                renderer.enabled = true;
                return true;
        }
        throw new System.Exception("Error!");
    }
}

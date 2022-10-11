using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private Animator animator;
    private SpriteRenderer sprRenderer;

    [SerializeField]
    private CharacterCollision upCol;
    [SerializeField]
    private CharacterCollision rightCol;
    [SerializeField]
    private CharacterCollision downCol;
    [SerializeField]
    private CharacterCollision leftCol;

    public Vector3 vector;

    private const float MOVE_SPEED = 250.0f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        sprRenderer.sortingOrder = (int)(transform.position.y * -1.0f * GameManager.DEPTH * GameManager.ANGLE / 90.0f);

        animator.SetBool("Walk", true);
        if (GameManager.Instance.pause != GameManager.PauseKind.None ||
            vector == Vector3.zero)
        {
            animator.SetBool("Walk", false);
            return;
        }
        Flip();
        CheckMove();
        Move();
    }

    private void Flip()
    {
        if (vector.x != 0)
        {
            sprRenderer.flipX = vector.x > 0;
        }
    }

    private void CheckMove()
    {
        if ((!upCol.canMove &&
            vector.y > 0) ||
            (!downCol.canMove &&
            vector.y < 0))
        {
            vector.y = 0;
        }
        if ((!rightCol.canMove &&
            vector.x == transform.localScale.x) ||
            (!leftCol.canMove &&
            vector.x != transform.localScale.x))
        {
            vector.x = 0;
        }
    }

    private void Move()
    {
        vector.Normalize();
        vector.y *= GameManager.ANGLE / 90.0f;
        transform.Translate(vector * MOVE_SPEED * Time.smoothDeltaTime);
    }
}

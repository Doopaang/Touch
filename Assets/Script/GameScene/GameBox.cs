using UnityEngine;

public class GameBox : MonoBehaviour
{
    protected SpriteRenderer sprRenderer;

    [SerializeField]
    private GameObject onThisObject;

    void Start()
    {
        sprRenderer = GetComponent<SpriteRenderer>();

        Init();
    }

    private void Init()
    {
        if (!sprRenderer.sprite)
        {
            return;
        }

        GameObject CalObject = null;
        SpriteRenderer CalRenderer = null;
        BoxCollider2D coll = GetComponent<BoxCollider2D>();
        if (onThisObject)
        {
            CalObject = onThisObject;
            CalRenderer = onThisObject.GetComponent<SpriteRenderer>();
        }
        else
        {
            CalObject = gameObject;
            CalRenderer = gameObject.GetComponent<SpriteRenderer>();
        }
        
        float size = CalRenderer.sprite.pivot.y * 2.0f;
        float pos = CalObject.transform.position.y;
        coll.size = Vector2.right * sprRenderer.sprite.rect.width + Vector2.up * size;
        coll.offset = Vector2.up * (CalObject.transform.position.y - transform.position.y);
        sprRenderer.sortingOrder += (int)((pos - Mathf.Abs(coll.size.y * 0.5f)) * -1.0f * GameManager.DEPTH * GameManager.ANGLE / 90.0f);
    }
}

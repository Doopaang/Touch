using UnityEngine;

public class CharacterCollision : MonoBehaviour
{
    [HideInInspector]
    public bool canMove;

    private GameObject obj = null;

    void Start()
    {
        canMove = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        obj = collision.gameObject;
        if (obj.tag == "Object" ||
            obj.tag == "Hint")
        {
            GameObj gameObj = obj.GetComponent<GameObj>();
            EventManager.Instance.SelectObj.Add(gameObj);
        }
        if (obj.tag == "Portal")
        {
            GameManager.Instance.Fade(Color.black, GameManager.FADE_SPEED, obj.GetComponent<GamePrt>());
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
            if (!renderer.sprite)
            {
                return;
            }
            if (obj.name == "BigDoor")
            {
                SfxManager.Instance.PlaySfx("BigDoor");
            }
            else
            {
                SfxManager.Instance.PlaySfx("Door");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        obj = collision.gameObject;
        if (obj.tag == "Wall" ||
            obj.tag == "Object")
        {
            canMove = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        obj = collision.gameObject;
        if (obj.tag == "Wall" ||
            obj.tag == "Object")
        {
            canMove = true;
        }
        if (obj.tag == "Object" ||
            obj.tag == "Hint")
        {
            GameObj gameObj = obj.GetComponent<GameObj>();
            gameObj.isHighlight = false;
            EventManager.Instance.SelectObj.Remove(gameObj);
        }
    }
}

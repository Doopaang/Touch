using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Camera mainCamera;
    [SerializeField]
    private GameObject menu;

    [HideInInspector]
    public PauseKind pause;
    public enum PauseKind
    {
        None, PopMsg, Puzzle, Fade, Menu
    }

    public enum Ending
    {
        HappyEnd, TimeOver, BadEnd
    }

    public const float ANGLE = 45.0f;
    public const int DEPTH = 10;

    public const float FADE_SPEED = 3.0f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        pause = PauseKind.None;
    }

    void Update()
    {
        InputControl();
    }

    public void Fade(Color color, float speed, GamePrt gamePrt)
    {
        GameObject init = new GameObject()
        {
            name = "Fader"
        };
        init.AddComponent<Fader>();
        Fader scr = init.GetComponent<Fader>();
        scr.Speed = speed;
        scr.Color = color;
        scr.Start = true;
        scr.GamePrt = gamePrt;

        pause = PauseKind.Fade;
    }

    public void End(Ending ending)
    {
        if (SceneManager.sceneCount > 1)
        {
            SceneManager.UnloadSceneAsync("PuzzleScene");
        }
        SceneManager.LoadScene("Ending" + ending.ToString());
    }

#if UNITY_STANDALONE_WIN
    private void InputControl()
    {
        switch (pause)
        {
            case PauseKind.None:
                Player.Instance.vector = Vector3.zero;
                if (Input.GetKey(KeyCode.W))
                {
                    Player.Instance.vector += Vector3.up;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    Player.Instance.vector += Vector3.down;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    Player.Instance.vector += Vector3.left;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    Player.Instance.vector += Vector3.right;
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    EventManager.Instance.PopEvent();
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    pause = PauseKind.Menu;
                    Cursor.visible = true;
                    menu.SetActive(true);
                }
                break;

            case PauseKind.PopMsg:
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A))
                {
                    MessageBox.Instance.SetChoice(true);
                }
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
                {
                    MessageBox.Instance.SetChoice(false);
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    MessageBox.Instance.NextPage();
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    MessageBox.Instance.Push();
                }
                break;

            case PauseKind.Puzzle:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    PuzzleManager.Instance.Action();
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    PuzzleManager.Instance.Fail();
                }
                break;

            case PauseKind.Menu:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Yes();
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    No();
                }
                break;
        }
    }
#endif

#if UNITY_ANDROID
    //public enum ControlKind
    //{
    //    Up, Left, Down, Right
    //}

    private void InputControl()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainScene");
        }
        switch (pause)
        {
            case PauseKind.None:
                float height = 2 * Camera.main.orthographicSize;
                float width = height * Camera.main.aspect;
                Vector3 cmrPos = Camera.main.transform.position;

                Vector3 tchPos = Input.mousePosition;
                tchPos.x = Mathf.Lerp(0.0f, width, tchPos.x / Screen.width) - width * 0.5f + cmrPos.x;
                tchPos.y = Mathf.Lerp(0.0f, height, tchPos.y / Screen.height) - height * 0.5f + cmrPos.y;

                Player.Instance.vector = Vector3.zero;
                if (Input.GetMouseButtonDown(0))
                {
                    foreach (GameObj obj in EventManager.Instance.SelectObj)
                    {
                        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();

                        if (CollisionPointToRect(tchPos, obj.transform.position, renderer.sprite))
                        {
                            EventManager.Instance.PopEvent();
                            return;
                        }
                    }
                }
                if (Input.GetMouseButton(0))
                {
                    Vector3 plrPos = Player.Instance.transform.position;
                    Player.Instance.vector = tchPos - plrPos;
                }
                break;

            case PauseKind.PopMsg:
                if (Input.GetMouseButtonDown(0))
                {
                    MessageBox.Instance.NextPage();
                }
                break;
        }
    }

    private bool CollisionPointToRect(Vector3 point, Vector3 pos, Sprite spr)
    {
        float xMin = pos.x - spr.pivot.x;
        float xMax = pos.x + (spr.rect.size.x - spr.pivot.x);
        float yMin = pos.y - spr.pivot.y;
        float yMax = pos.y + (spr.rect.size.y - spr.pivot.y);

        if (point.x >= xMin && point.x <= xMax &&
            point.y >= yMin && point.y <= yMax)
        {
            return true;
        }
        return false;
    }

    //public void ActionClick()
    //{
    //    switch (pause)
    //    {
    //        case PauseKind.None:
    //            EventManager.Instance.PopEvent();
    //            break;

    //        case PauseKind.PopMsg:
    //            MessageBox.Instance.NextPage();
    //            break;
    //    }
    //}
    //public void CancelClick()
    //{
    //    switch (pause)
    //    {
    //        case PauseKind.PopMsg:
    //            MessageBox.Instance.Push();
    //            break;
    //    }
    //}
#endif

    public void Yes()
    {
        Cursor.visible = false;
        SceneManager.LoadScene("MainScene");
    }

    public void No()
    {
        Cursor.visible = false;
        menu.SetActive(false);
        pause = PauseKind.None;
    }
}

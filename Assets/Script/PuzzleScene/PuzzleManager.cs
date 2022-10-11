using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; }

    public Camera subCamera;
    public Transform canvas;

    [SerializeField]
    private List<GameObject> prefabs;
    private Dial dial;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        foreach (GameObject obj in prefabs)
        {
            if (obj.name == "Dial_" + MessageBox.Instance.key)
            {
                GameObject gameObj=Instantiate(obj, canvas);
                dial = gameObj.GetComponent<Dial>();
                break;
            }
        }
    }

    void Update()
    {
        subCamera.transform.position = GameManager.Instance.mainCamera.transform.position;
    }

    public void Action()
    {
        if(dial == null)
        {
            Fail();
        }
        for (int count = 0; count < dial.answerText.Count; count++)
        {
            if (dial.answer[count] != MessageBox.Instance.puzzleAnswer[count] - 48)
            {
                Fail();
                return;
            }
        }
        Success();
    }
    
    public void Success()
    {
        GameManager.Instance.pause = GameManager.PauseKind.PopMsg;
        MessageBox.Instance.NextPage();
        if (MessageBox.Instance.key == "toy box")
        {
            SfxManager.Instance.PlaySfx("chest");
        }
        if (MessageBox.Instance.key == "drawer2")
        {
            SfxManager.Instance.PlaySfx("pad");
        }
        dial = null;
        SceneManager.UnloadSceneAsync("PuzzleScene");
    }

    public void Fail()
    {
        MessageBox.Instance.Push();
        dial = null;
        SceneManager.UnloadSceneAsync("PuzzleScene");
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MessageBox : MonoBehaviour
{
    public static MessageBox Instance { get; private set; }
    private Text text;
    [SerializeField]
    private GameObject button;

    [HideInInspector]
    public Data data;
    public Data BeforeData { get; private set; }
    public Data AfterData { get; private set; }
    public Data EventData { get; private set; }
    private GameObject image;

    private Message curMsg;
    private int curPage;
    [HideInInspector]
    public int curChoice;
    
    [HideInInspector]
    public string puzzleAnswer;
    [HideInInspector]
    public string key;

    private enum PageType
    {
        Messsage, Image, Ending, Puzzle
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        text = GetComponentInChildren<Text>();

        TextAsset str = Resources.Load("Data/BeforeMessage", typeof(TextAsset)) as TextAsset;
        BeforeData = JsonUtility.FromJson<Data>(str.text);

        str = Resources.Load("Data/AfterMessage", typeof(TextAsset)) as TextAsset;
        AfterData = JsonUtility.FromJson<Data>(str.text);

        str = Resources.Load("Data/EventMessage", typeof(TextAsset)) as TextAsset;
        EventData = JsonUtility.FromJson<Data>(str.text);

        gameObject.SetActive(false);
    }

    public void Pop(string key)
    {
        this.key = key;
        switch (EventManager.Instance.start)
        {
            case EventManager.TimeKind.Before:
                data = BeforeData;
                break;

            case EventManager.TimeKind.After:
                data = AfterData;
                break;

            default:
                data = EventData;
                break;
        }

        curMsg = data.Find(key);

        if (curMsg == null)
        {
            return;
        }

        curPage = 0;
        curChoice = 0;

        GameManager.Instance.pause = GameManager.PauseKind.PopMsg;

        Print();
    }

    public void Push()
    {
        if (GameManager.Instance.pause != GameManager.PauseKind.PopMsg &&
            GameManager.Instance.pause != GameManager.PauseKind.Puzzle)
        {
            return;
        }

        Cursor.visible = false;
        if (image)
        {
            Destroy(image);
            button.SetActive(false);
        }
        if (GameManager.Instance.pause == GameManager.PauseKind.Puzzle)
        {
            SceneManager.UnloadSceneAsync("PuzzleScene");
        }
        
        if (curMsg.flags[curMsg.flag].contents[curPage].flag != -1)
        {
            curMsg.flag = curMsg.flags[curMsg.flag].contents[curPage].flag;
        }

        gameObject.SetActive(false);
        GameManager.Instance.pause = GameManager.PauseKind.None;

        switch (EventManager.Instance.start)
        {
            case EventManager.TimeKind.Start:
                if(curMsg.flags[0].contents[curPage].flag < 0)
                {
                    break;
                }
                EventManager.Instance.start = EventManager.TimeKind.Before;
                EventManager.Instance.BeforeTimeStart();
                break;

            case EventManager.TimeKind.Over:
                EventManager.Instance.start = EventManager.TimeKind.After;
                EventManager.Instance.AfterTimeStart(EventData.Find("gate").flag);
                break;
        }
    }

    public void Print()
    {
        Page page = curMsg.flags[curMsg.flag].contents[curPage];
        switch (page.type)
        {
            case (int)PageType.Messsage:
                string str = page.message;

                if (str == "")
                {
                    Push();
                    return;
                }

                if (page.choice.Count > 0)
                {
                    for (int count = 0; count < 5 - page.choice.Count; count++)
                    {
                        if (page.choice.Count > 0)
                        {
                            for (int index = 0; index < page.choice.Count; index++)
                            {
                                if (page.choice[index].text.Length > 50)
                                {
                                    count++;
                                }
                            }
                        }
                        str += '\n';
                    }
                    for (int index = 0; index < page.choice.Count; index++)
                    {
                        if (page.choice[index].text != "")
                        {
                            str += index == curChoice ? "▶ " : "― ";
                        }
                        str += page.choice[index].text + '\n';
                    }
                }
                text.text = str;

                gameObject.SetActive(true);
                break;

            case (int)PageType.Puzzle:
                puzzleAnswer = page.message;
                SceneManager.LoadScene("PuzzleScene", LoadSceneMode.Additive);

                GameManager.Instance.pause = GameManager.PauseKind.Puzzle;

                Cursor.visible = true;
                gameObject.SetActive(false);
                break;

            case (int)PageType.Image:
                GameManager.Instance.pause = GameManager.PauseKind.PopMsg;

                Vector3 position = Camera.main.transform.position;
                position.z = 0;

                image = new GameObject(page.message);
                image.transform.position = position;
                SpriteRenderer sprRenderer = image.AddComponent<SpriteRenderer>();

                string path = "Image/" + page.message;
                sprRenderer.sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
                sprRenderer.sortingLayerName = "UI";
                if(page.message == "book" ||
                    page.message == "Chart_c" ||
                    page.message == "Chart_H" ||
                    page.message == "Chart_N" ||
                    page.message == "Hint")
                {
                    SfxManager.Instance.PlaySfx("page");
                }

                image.AddComponent<GameImg>();

                gameObject.SetActive(false);

                Cursor.visible = true;
                button.SetActive(true);
                position.x += sprRenderer.sprite.rect.size.x * 0.5f;
                position.y += sprRenderer.sprite.rect.size.y * 0.5f;
                button.transform.position = position;
                break;

            case (int)PageType.Ending:
                GameManager.Instance.End((GameManager.Ending)int.Parse(page.message));

                gameObject.SetActive(false);
                GameManager.Instance.pause = GameManager.PauseKind.None;
                break;
        }
    }

    public void NextPage()
    {
        if (image)
        {
            button.SetActive(false);
            Destroy(image);
            image = null;
        }

        Page page = curMsg.flags[curMsg.flag].contents[curPage];
        if (page.choice.Count == 0)
        {
            Push();
            return;
        }

        curPage = page.choice[curChoice].page;
        curChoice = 0;

        Print();
    }

    public void SetChoice(bool isUp)
    {
        Page page = curMsg.flags[curMsg.flag].contents[curPage];
        if (page.choice.Count <= 1)
        {
            return;
        }

        if (isUp)
        {
            curChoice--;
            if (curChoice < 0)
            {
                curChoice = page.choice.Count - 1;
            }
        }
        else
        {
            curChoice++;
            if (curChoice > page.choice.Count - 1)
            {
                curChoice = 0;
            }
        }

        Print();
    }

    [System.Serializable]
    public class Data
    {
        public List<Message> messages;

        public Message Find(string key)
        {
            foreach (Message msg in messages)
            {
                if (msg.key == key)
                {
                    return msg;
                }
            }
            return null;
        }
    }

    [System.Serializable]
    public class Message
    {
        public string key;
        public int flag;
        public List<Flag> flags;
    }
    [System.Serializable]
    public struct Flag
    {
        public List<Page> contents;
    }
    [System.Serializable]
    public struct Page
    {
        public int type;
        public string message;
        public List<Choice> choice;
        public int flag;
    }
    [System.Serializable]
    public struct Choice
    {
        public int page;
        public string text;
    }
}

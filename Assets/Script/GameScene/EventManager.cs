using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public List<GameObj> SelectObj { get; private set; }
    private int effective = 0;

    public List<GameRoom> Rooms { get; private set; }

    [HideInInspector]
    public TimeKind start;

    private const float BEFORE_TIME = 450.0f;
    private const float AFTER_TIME = 3.0f;
    private const float PLUS_TIME = 27.0f;

    public enum TimeKind
    {
        Start, Before, Over, After
    }

    void Awake()
    {
        Instance = this;

        SelectObj = new List<GameObj>();
        Rooms = new List<GameRoom>();
    }

    void Start()
    {
        start = TimeKind.Start;
    }

    void Update()
    {
        CheckSelectObject();

        if (MessageBox.Instance.BeforeData.Find("toy box").flag > 0 &&
            MessageBox.Instance.BeforeData.Find("drawer2").flag > 0)
        {
            if (start == TimeKind.Before)
            {
                CancelInvoke("BeforeTimeOver");
                BeforeTimeOver();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameManager.Instance.pause == GameManager.PauseKind.None)
        {
            BoxCollider2D coll = GetComponent<BoxCollider2D>();
            coll.enabled = false;

            MessageBox.Instance.Pop("gate");
        }
    }

    private void CheckSelectObject()
    {
        if (SelectObj.Count == 0)
        {
            return;
        }

        int tag = 0;
        effective = 0;
        foreach (GameObj obj in SelectObj)
        {
            SelectObj[tag].isHighlight = false;

            if (effective != tag)
            {
                Collider2D objColl = obj.GetComponent<Collider2D>();
                Collider2D effColl = SelectObj[effective].GetComponent<Collider2D>();

                Vector3 plrPos = Player.Instance.gameObject.transform.position;
                Vector3 objPos = obj.transform.position + Vector3.right * objColl.offset.x + Vector3.up * objColl.offset.y;
                Vector3 effPos = SelectObj[effective].transform.position + Vector3.right * effColl.offset.x + Vector3.up * effColl.offset.y;

                Vector3 objDirect = objPos - plrPos;
                Vector3 effDirect = effPos - plrPos;

                int plrSide = Player.Instance.gameObject.transform.localScale.x > 0 ? 1 : -1;
                int objSide = objDirect.x > 0 ? 1 : -1;
                int effSide = effDirect.x > 0 ? 1 : -1;

                // 같은 방향일 때
                if (objSide == effSide)
                {
                    // 더 가까울 때
                    if (objDirect.sqrMagnitude < effDirect.sqrMagnitude)
                    {
                        effective = tag;
                    }
                }
                else
                {
                    // 같은 방향일 때
                    if (objSide == plrSide)
                    {
                        effective = tag;
                    }
                }
            }
            tag++;
        }
        SelectObj[effective].isHighlight = true;
    }

    public void PopEvent()
    {
        if (SelectObj.Count == 0)
        {
            return;
        }

        MessageBox.Instance.Pop(SelectObj[effective].key);
    }

    public void BeforeTimeStart()
    {
        Invoke("BeforeTimeOver", BEFORE_TIME);
    }

    private void BeforeTimeOver()
    {
        MessageBox.Instance.Push();

        start = TimeKind.Over;

        SfxManager.Instance.PlaySfx("Walking");
        MessageBox.Instance.Pop("timeover");
    }

    public void AfterTimeStart(int flag)
    {
        Invoke("AfterTimeOver", AFTER_TIME + (flag != 0 ? PLUS_TIME : 0.0f));
    }

    private void AfterTimeOver()
    {
        MessageBox.Instance.Push();

        GameManager.Instance.End(GameManager.Ending.TimeOver);
    }
}

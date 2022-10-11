using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutManager : MonoBehaviour
{
    [SerializeField]
    private string nextScene;
    [SerializeField]
    private List<GameObject> cuts;
    private int now;

    private Cut nowCut;
    
    private void Start()
    {
        now = 0;

        Cursor.visible = false;

        nowCut = cuts[now].GetComponent<Cut>();
        nowCut.Action();
    }

    void Update()
    {
        if (!nowCut.wait &&
            now < cuts.Count - 1)
        {
            now++;
            nowCut = cuts[now].GetComponent<Cut>();
            nowCut.Action();
        }

#if UNITY_STANDALONE_WIN
        if (Input.GetKeyDown(KeyCode.Space))
        {
#endif
#if UNITY_ANDROID
        if (Input.GetMouseButtonDown(0))
        {
#endif
            if (!nowCut.wait)
            {
                if (now == cuts.Count - 1)
                {
                    SceneManager.LoadScene(nextScene);
                    return;
                }
            }
            else
            {
                nowCut.Skip();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}

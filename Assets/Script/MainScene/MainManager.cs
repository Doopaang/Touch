using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    void Start()
    {
        Screen.SetResolution(Screen.width, Screen.width / 16 * 9, Screen.fullScreen);

        Cursor.visible = true;
    }
    
    public void ChangeGameScene()
    {
        SceneManager.LoadScene("IntroScene");
    }

    public void ChangeCollectionScene()
    {
        SceneManager.LoadScene("CreditScene");
    }

    public void Exit()
    {
        Application.Quit();
    }
}

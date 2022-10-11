using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }
}

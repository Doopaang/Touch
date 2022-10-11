using UnityEngine;

public class Fader : MonoBehaviour
{
    public bool Start { private get; set; }
    public float Speed { private get; set; }
    private float alpha = 0.0f;
    public Color Color { private get; set; }
    private bool isFadeIn = false;
    public GamePrt GamePrt { private get; set; }
    
    void OnGUI()
    {
        if (!Start)
        {
            return;
        }

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        Texture2D myTex;

        myTex = new Texture2D(1, 1);
        myTex.SetPixel(0, 0, Color);
        myTex.Apply();

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), myTex);
        // 검은 화면
        if (alpha >= 1.0f)
        {
            GamePrt.ChangeRoom();
            isFadeIn = true;
        }
        // 페이드 끝
        else if (alpha < 0.0f)
        {
            if (GameManager.Instance.pause == GameManager.PauseKind.Fade)
            {
                GameManager.Instance.pause = GameManager.PauseKind.None;
            }
            Destroy(gameObject);
        }
        // 페이드 인
        if (isFadeIn)
        {
            alpha = Mathf.Lerp(alpha, -0.1f, Speed*Time.deltaTime);
        }
        // 페이드 아웃
        else
        {
            alpha = Mathf.Lerp(alpha, 1.1f, Speed*Time.deltaTime);
        }
    }
}

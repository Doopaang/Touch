using UnityEngine;

public class GameObj : GameBox
{
    public string key;
    [HideInInspector]
    public bool isHighlight;
    
    void Update()
    {
        SetHighlight();
    }

    private void SetHighlight()
    {
        if (isHighlight)
        {
            sprRenderer.material.shader = Shader.Find("Highlight/Outline");
        }
        else
        {
            sprRenderer.material.shader = Shader.Find("Sprites/Default");
        }
    }

}

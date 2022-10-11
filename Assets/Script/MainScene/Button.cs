using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    private Text text;

    void Start()
    {
        text = gameObject.GetComponentInChildren<Text>();
        text.color = Color.black;
    }
    
    private void OnMouseEnter()
    {
        text.color = Color.gray;
    }
    
    private void OnMouseDown()
    {
        Color color = Color.black;
        color.r = 0.8f;
        color.g = 0.8f;
        color.b = 0.8f;
        text.color = color;
    }

    private void OnMouseExit()
    {
        text.color = Color.black;
    }
}

using UnityEngine;

public class GameImg : MonoBehaviour
{
    public float duration = 1.0f;
    private float startTime;
    public SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        startTime = Time.time;
        Fade();
    }

    void Update()
    {
        Fade();
    }

    void Fade()
    {
        float t = (Time.time - startTime) / duration;
        sprite.material.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(0.0f, 1.0f, t));
    }
}

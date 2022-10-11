using UnityEngine;

public class BgmManager : MonoBehaviour
{
    public static BgmManager Instance { get; private set; }
    private AudioSource source;

    public AudioClip background;
    public AudioClip after;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void SetBgm(string name)
    {
        switch (name)
        {
            case "background":
                source.clip = background;
                break;

            case "after":
                source.clip = after;
                break;
        }
    }
}

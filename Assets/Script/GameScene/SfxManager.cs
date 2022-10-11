using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public static SfxManager Instance { get; private set; }
    private AudioSource source;

    public AudioClip bigDoor;
    public AudioClip door;
    public AudioClip walking;
    public AudioClip dial;
    public AudioClip chest;
    public AudioClip pad;
    public AudioClip page;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySfx(string name)
    {
        switch (name)
        {
            case "Door":
                source.clip = door;
                break;

            case "Walking":
                source.clip = walking;
                break;

            case "BigDoor":
                source.clip = bigDoor;
                break;

            case "Dial":
                source.clip = dial;
                break;

            case "chest":
                source.clip = chest;
                break;

            case "pad":
                source.clip = pad;
                break;

            case "page":
                source.clip = page;
                break;
        }
        source.Play();
    }
}

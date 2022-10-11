using UnityEngine;

public class GameRoom : MonoBehaviour
{
    public int roomID;

    void Start()
    {
        EventManager.Instance.Rooms.Add(this);
    }
}

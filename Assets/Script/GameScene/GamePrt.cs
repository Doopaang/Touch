using UnityEngine;

public class GamePrt : GameBox
{
    public int roomID;
    [SerializeField]
    private Vector2 goalPosition;
    [SerializeField]
    private bool ignoreX;
    [SerializeField]
    private bool ignoreY;
    
    public void ChangeRoom()
    {
        Vector2 roomPos = Vector2.zero;
        foreach (GameRoom temp in EventManager.Instance.Rooms)
        {
            if (temp.roomID == roomID)
            {
                roomPos = temp.gameObject.transform.position;
            }
        }
        Vector2 plrPos = Player.Instance.transform.position - gameObject.transform.position;
        if (ignoreX)
        {
            plrPos.x = 0;
        }
        if (ignoreY)
        {
            plrPos.y = 0;
        }
        Player.Instance.transform.position = roomPos + goalPosition + plrPos;

        Camera.main.transform.position = (Vector3)roomPos + Vector3.forward * Camera.main.transform.position.z;
    }
}

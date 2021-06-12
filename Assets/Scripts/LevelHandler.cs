using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelHandler : MonoBehaviour
{
    public Map map;
    public Vector2 StartTile;
    public Transform Player;
    public CameraController CC;

    public Text RoomID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalcRoomID();
    }

    public void SetPlayer(Vector2 StartTile)
    {
        this.StartTile = StartTile;
        Player.position = StartTile + new Vector2(0, 2);
        Player.transform.GetComponent<Rigidbody2D>().isKinematic = false;
    }

    public void Setup(Vector2 StartTile, float RoomWidth, float RoomHeight)
    {
        this.StartTile = StartTile;
        if(StartTile.x != 0)SetPlayer(StartTile);
        CC.CameraSetup(RoomWidth, RoomHeight);
    }

    public void Setup(Map map)
    {
        this.map = map;
        Setup(new Vector2(map.start.x, -map.start.y), map.dimensions.room_width, map.dimensions.room_height);
    }

    public void CalcRoomID()
    {
        Vector2 cameraPos = CC.transform.position;
        int x = (int)(cameraPos.x / map.dimensions.room_width);
        int y = -(int)(cameraPos.y / map.dimensions.room_height);

        //RoomID.text = "(" + x + ", " + y + ")";
        int roomID = 1 + x + y * map.dimensions.room_count_width;
        RoomID.text = roomID.ToString();
    }
}

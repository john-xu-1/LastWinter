using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WorldBuilder;

public class DungeonHandler : MonoBehaviour
{
    public Worlds worlds;
    public GameObject playerPrefab;

    public InventoryObjects[] allItems;

    public CameraController camControl;
    public MapGenerator mapG;

    private World dungeon;

    private Transform player;

    private int worldWidth, worldHeight, roomWidth, roomHeight;

    void MapSetup()
    {
        dungeon = worlds.GetWorld();
       

        mapG.BuildWorld(dungeon);

        worldWidth = dungeon.Width;
        worldHeight = dungeon.Height;
        roomWidth = dungeon.GetRooms()[0].map.dimensions.room_width;
        roomHeight = dungeon.GetRooms()[0].map.dimensions.room_height;
    }

    void PlayerSetup()
    {
        player = Instantiate(playerPrefab, new Vector3(dungeon.startPos.x, dungeon.startPos.y, 0), Quaternion.identity).transform;
    }

    private void Start()
    {
        MapSetup();
        PlayerSetup();
        camSetup();
    }

    void camSetup()
    {
        camControl.setUp(worldWidth, worldHeight, roomWidth, roomHeight, player);
    }

    private void Update()
    {
        
    }
}

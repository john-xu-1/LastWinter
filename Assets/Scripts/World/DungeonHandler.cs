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

    public GameObject wepChipPan;
    public GameObject invPan;


    private int worldWidth, worldHeight, roomWidth, roomHeight;

    public void MapSetup(World map)
    {
        //dungeon = worlds.GetWorld();

        dungeon = map;

        mapG.BuildWorld(dungeon);

        worldWidth = dungeon.Width;
        worldHeight = dungeon.Height;
        roomWidth = dungeon.GetRooms()[0].map.dimensions.room_width;
        roomHeight = dungeon.GetRooms()[0].map.dimensions.room_height;
    }

    public void PlayerSetup()
    {
        player = Instantiate(playerPrefab, new Vector3(dungeon.startPos.x, dungeon.startPos.y, 0), Quaternion.identity).transform;
        player.GetComponent<InventorySystem>().wepChipPanel = wepChipPan;
        invPan.SetActive(true);
    }

    private void Start()
    {
        invPan.SetActive(false);
    }

    public void camSetup()
    {
        camControl.setUp(worldWidth, worldHeight, roomWidth, roomHeight, player);
    }

    private void Update()
    {
        
        
    }
}

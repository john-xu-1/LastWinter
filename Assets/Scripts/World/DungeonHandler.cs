using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public GameObject DebugMenu;

    public int worldWidth, worldHeight, roomWidth, roomHeight;
    private bool buildingMap;

    public bool setupComplete = false;

    public bool debugging = false;
    public List<ChunkHandler.Chunk> chunks;

    public bool buildMap;

    public void MapSetup(int mapIndex)
    {
        MapSetup(worlds.BuiltWorlds[mapIndex]);
    }
    public void MapSetup(World map)
    {
        //dungeon = worlds.GetWorld();

        dungeon = map;

        if(buildMap) mapG.BuildWorld(dungeon);
        else chunks = mapG.BuildWorldChunks(dungeon);

        worldWidth = dungeon.Width;
        worldHeight = dungeon.Height;
        roomWidth = dungeon.GetRooms()[0].map.dimensions.room_width;
        roomHeight = dungeon.GetRooms()[0].map.dimensions.room_height;

        buildingMap = true;

    }

    public void PlayerSetup()
    {
        Debug.Log("PlayerSetup()");
        player = Instantiate(playerPrefab, new Vector3(dungeon.startPos.x + 0.5f, -dungeon.startPos.y + 1, 0), Quaternion.identity).transform;
        //player.GetComponent<InventorySystem>().wepChipPanel = wepChipPan;
        SetUpPlayerUI();
    }

    public void SetUpPlayerUI()
    {
        if (invPan) invPan.SetActive(true);
    }

    private void Start()
    {
        //invPan.SetActive(false);
    }

    public void camSetup()
    {
        camControl.setUp(worldWidth, worldHeight, roomWidth, roomHeight, player);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) DebugMenu.SetActive(!DebugMenu.activeSelf);
        if (buildingMap && !mapG.BuildingRooms)
        {
            Debug.Log("Dungeonhandler");
            setupComplete = true;
            if (!debugging)
            {
                PlayerSetup();
                camSetup();
                FindObjectOfType<LightingLevelSetup>().setupLighting(worldWidth * roomWidth, worldHeight * roomHeight);

                InventorySystem inventorySystem = GameObject.FindObjectOfType<InventorySystem>();

                foreach (Pickupable pickupable in GameObject.FindObjectsOfType<Pickupable>())
                {
                    pickupable.setInventorySystem(inventorySystem);
                }

                //FindObjectOfType<CameraController>().active = true;
            }

            if (buildMap) FindObjectOfType<PathFinder>().SetMap(0, -worldHeight * roomHeight, worldWidth * roomWidth, 0);
            
            buildingMap = false;
            
        }

    }

    public void ReloadSceneButton()
    {
        int sceneID = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneID);
    }

    public void ToggelFullScreenButton()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void SnapToRoomButton()
    {
        camControl.setUp(camControl.FollowType != CameraController.CameraFollow.follow);
    }
}


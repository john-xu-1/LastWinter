using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using WorldBuilder;

public class DebugMap : MonoBehaviour
{
    public MapGenerator Generator;
    public int[][] MyMap;

    [TextArea(5, 10)]
    public string Bitmap;

    public string JsonMapName = "test2.txt";
    public TextAsset jsonFile;
    public bool BuildOnStart;
    public ClingoSolver Solver;
    public Worlds WorldBuilder;
    public bool[] connection = { true, true, false, false, false, false, false, false };
    public RoomConnections connections;
    public Vector2Int RoomSize = new Vector2Int(20, 20);
    public int headroom = 2, shoulderroom = 3, minCeilingHeight = 3;
    public GameObject nodePrefab, edgePrefab;

    public int worldWidth = 4, worldHeight = 4, keyTypeCount = 3, maxGatePerKey = 2;
    public int jumpHeadroom = 3, timeout = 600;
    public int BuiltWorldIndex;
    public enum MapSources
    {
        None,
        Json,
        Solver,
        World
    }
    public MapSources MapSource;
    public World world;

    private void Start()
    {

        

        if (BuildOnStart) buildMap();
    }
    bool isBuilt = false;
    int xTest = 0;
    int yTest = 0;
    Room lastRoom;
    private void Update()
    {
        
    }

    private void buildMap()
    {
        if(MapSource == MapSources.Json)
        {
            //TextAsset jsonFile = Resources.Load<TextAsset>(JsonMapName);
            print(jsonFile);
            string jsonStr = jsonFile.text;
            Map map = JsonUtility.FromJson<Map>(jsonStr);
            print(map.dimensions.room_count_height);
            //Generator.ConvertMap(map);
        }else if(MapSource == MapSources.Solver)
        {
            //Generator.ConvertMap(Solver.answerSet);
            FindObjectOfType<BuildWorld>().BuildAWorld(worldWidth, worldHeight, keyTypeCount, maxGatePerKey, RoomSize.x, RoomSize.y, headroom, shoulderroom, jumpHeadroom, timeout);
        }
        else if (MapSource == MapSources.World)
        {
            //WorldBuilder.BuildWorld(worldWidth, worldHeight, keyTypeCount, maxGatePerKey, 3, Solver.maxDuration - 10);
            World world = WorldBuilder.BuiltWorlds[BuiltWorldIndex];
            //WorldMap.ConvertGraph()
            foreach(Room room in world.GetRooms())
            {
                room.SetupRoom();
                Generator.ConvertMap(room);
            }
        }


    }
    
    
}



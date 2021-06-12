using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
    public bool[] connections = { true, true, false, false, false, false, false, false };
    public Vector2Int RoomSize = new Vector2Int(20, 20);
    public int headroom = 2, shoulderroom = 3, minCeilingHeight = 3;
    public GameObject nodePrefab, edgePrefab;

    public int worldWidth = 4, worldHeight = 4, keyTypeCount = 3, maxGatePerKey = 2;
    public enum MapSources
    {
        Json,
        Solver,
        World
    }
    public MapSources MapSource;

    private void Start()
    {
        
        

        if (BuildOnStart) buildMap();
    }
    bool isBuilt = false;
    int xTest = 0;
    Room lastRoom;
    private void Update()
    {
        if(Solver.SolverStatus == ClingoSolver.Status.SATISFIABLE && !isBuilt)
        {
            if(MapSource == MapSources.Solver)
            {
                //WorldBuilder.AddWorld(Solver.answerSet);
                lastRoom = new Room(new Vector2Int(xTest, 0), Solver.answerSet);
                Generator.ConvertMap(lastRoom);


                if (xTest < 5)
                {
                    Neighbors neighbors = new Neighbors();
                    neighbors.left = lastRoom;
                    buildMap(neighbors);
                }
                else
                    isBuilt = true;
                xTest += 1;
            }else if(MapSource == MapSources.World)
            {
                WorldMap.DisplayGraph(Solver.answerSet, nodePrefab, edgePrefab);
                RoomConnections[] connections = WorldMap.get_room_connections(Solver.answerSet);
                for(int i = 0; i < connections.Length; i += 1)
                {
                    Debug.Log(i + " " + connections[i]);
                }
                isBuilt = true;
            }
            
            
        }
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

            WorldBuilder.BuildRoom(RoomSize, headroom,shoulderroom,minCeilingHeight, connections, new Neighbors());
        }
        else if (MapSource == MapSources.World)
        {
            WorldBuilder.BuildWorld(worldWidth, worldHeight, keyTypeCount, maxGatePerKey, 3, Solver.maxDuration - 10);
        }


    }
    private void buildMap(Neighbors neighbors)
    {
        WorldBuilder.BuildRoom(RoomSize, headroom, shoulderroom, minCeilingHeight, connections, neighbors);
    }
    
}



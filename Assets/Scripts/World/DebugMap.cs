﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

using WorldBuilder;
using Clingo;


public class DebugMap : MonoBehaviour
{
    public Text RoomID;
    public Text TotalTime;
    public Text BuildTime;
    public Text RunNum;

    public MapGenerator Generator;
    public int[][] MyMap;

    [TextArea(5, 10)]
    public string Bitmap;

    public string JsonMapName = "test2.txt";
    public TextAsset jsonFile;
    public bool BuildOnStart;
    public ClingoSolver Solver;
    public int cpus = 4;
    public Worlds WorldBuilder;
    public bool[] connection = { true, true, false, false, false, false, false, false };
    public RoomConnections connections;
    public Vector2Int RoomSize = new Vector2Int(20, 20);
    public int headroom = 2, shoulderroom = 3, minCeilingHeight = 3;
    public GameObject nodePrefab, edgePrefab;

    public int worldWidth = 4, worldHeight = 4, keyTypeCount = 3, maxGatePerKey = 2;
    public int jumpHeadroom = 3, timeout = 600;
    public int BuiltWorldIndex;
    public int[] indices = { 1, 2, 3, 4 };
    public List<List<int>> permutations;
    public enum MapSources
    {
        None,
        Json,
        Solver,
        World,
        History
    }
    public MapSources MapSource;
    public World world;

    private void Start()
    {
        //Debug.Log(Clingo.ClingoSolver.Status.SATISFIABLE);
        //Clingo.ClingoSolver.Status status = "SATISFIABLE";
        //if("SATISFIABLE" == Clingo.ClingoSolver.Status.SATISFIABLE.ToString())
        //{

        //}
        //permutations = global::WorldBuilder.Utility.GetPermutations( indices);
        //foreach (List<int> permutation in permutations)
        //{
        //    string line = "";
        //    foreach (int index in permutation)
        //    {
        //        line += index;
        //    }
        //    Debug.Log(line);
        //}
        //int count = permutations.Count;
        //for(int i = 0; i < count; i += 1)
        //{


        //    List<int> removed = global::WorldBuilder.Utility.GetSmallestRandomPermutation(permutations, true);
        //    string line2 = "";
        //    foreach (int index in removed)
        //    {
        //        line2 += index;
        //    }
        //    Debug.Log("Removed: " + line2);
        //}

        for (int i = 0; i < 11; i += 1)
        {
            WorldBuilder.BuiltWorlds.RemoveAt(10);
        }

        if (BuildOnStart) buildMap();
    }
    bool isBuilt = false;
    int xTest = 0;
    int yTest = 0;
    Room lastRoom;
    World historySource;
    World worldHistory;
    int historyIndex = 0;
    public GameObject MiniMap;
    private void Update()
    {
        if(MapSource == MapSources.History)
        {
            
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                historyIndex += 1;
                RoomHistory rh = historySource.WorldHistory.GetRoom(historyIndex);
                Room room = worldHistory.GetRoom(rh.roomID);
                if (rh.destroyed)
                {
                    Debug.Log("RemoveMap");
                    Generator.RemoveMap(room);
                }
                else
                {
                    room.SetupRoom(rh.map);
                    Generator.ConvertMap(room);
                }
                RoomID.text = rh.roomID.ToString();
                BuildTime.text = Utility.FormatTime(rh.buildTime);
                RunNum.text = (historyIndex + 1).ToString();

            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                
                RoomHistory rh = historySource.WorldHistory.GetRoom(historyIndex);
                if (!rh.destroyed)
                {
                    Room room = worldHistory.GetRoom(rh.roomID);
                    Debug.Log("RemoveMap");
                    Generator.RemoveMap(room);
                }else 
                {
                    Room room = worldHistory.GetRoom(rh.roomID);
                    Generator.ConvertMap(room);
                }

                historyIndex -= 1;

                RoomID.text = historySource.WorldHistory.GetRoom(historyIndex).roomID.ToString();
                BuildTime.text = Utility.FormatTime(historySource.WorldHistory.GetRoom(historyIndex).buildTime);
                RunNum.text = (historyIndex + 1).ToString();

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
            FindObjectOfType<BuildWorld>().BuildAWorld(worldWidth, worldHeight, keyTypeCount, maxGatePerKey, RoomSize.x, RoomSize.y, headroom, shoulderroom, jumpHeadroom, timeout, cpus);
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
        }else if(MapSource == MapSources.History)
        {


            historySource = WorldBuilder.BuiltWorlds[BuiltWorldIndex];

            WorldMap.DisplayGraph(historySource.worldGraph, nodePrefab, edgePrefab, MiniMap.transform);

            worldHistory = new World(historySource.Width, historySource.Height);
            historyIndex = 0;
            RoomHistory rh = historySource.WorldHistory.GetRoom(historyIndex);
            Room room = worldHistory.GetRoom(rh.roomID);
            room.SetupRoom(rh.map);
            Generator.ConvertMap(room);

            TotalTime.text = Utility.FormatTime(historySource.WorldHistory.GetTotalTime());
            RoomID.text = rh.roomID.ToString();
            BuildTime.text = ((int)rh.buildTime).ToString();
            RunNum.text = (historyIndex + 1).ToString();

            historySource.WorldHistory.GetRoomHistoryAnalysis();
        }


    }
    
    
}


using System.Collections;
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
    public Worlds WorldBuilder, WorldsSource;
    public bool[] connection = { true, true, false, false, false, false, false, false };
    public RoomConnections connections;
    public Vector2Int RoomSize = new Vector2Int(20, 20);
    public int headroom = 2, shoulderroom = 3, minCeilingHeight = 3;
    public GameObject nodePrefab, edgePrefab, pathPrefab;

    public int worldWidth = 4, worldHeight = 4, keyTypeCount = 3, maxGatePerKey = 2, minGatePerKey = 2, bossGateKey = 2;
    public int jumpHeadroom = 3, timeout = 600;
    public int BuiltWorldIndex;
    int builtWorldIndex { get { return BuiltWorldIndex >= 0 ? Mathf.Min(BuiltWorldIndex, WorldBuilder.BuiltWorlds.Count - 1) : Mathf.Max(WorldBuilder.BuiltWorlds.Count + BuiltWorldIndex, 0); } }
    public int[] indices = { 1, 2, 3, 4 };
    public List<List<int>> permutations;
    public enum MapSources
    {
        None,
        Json,
        Solver,
        World,
        History,
        Graph
    }
    public MapSources MapSource;
    public World world;

    //List<List<string>> graphRuntimeData = new List<List<string>>();
    string[,] graphRuntimeData;
    public int[] cpuDebugs = { 1, 2, 4, 8 };
    public int[] widthDebugs = { 20 }, heightDebugs = { 20 };
    public int[] keyTypesDebugs = { 2 };
    int cpuDebugsIndex = 0;
    int runID = 0;
    public void GraphRuntimeData(float value)
    {
        GraphRuntimeData(value, runID, cpuDebugsIndex, 0, 0, 0);
    }
    public void GraphRuntimeData(float value, int runID, int cpu, int width, int height, int keyType)
    {
        
        graphRuntimeData[cpu,runID] = value.ToString();
        Debug.Log($"x:{cpu},y:{runID} = {graphRuntimeData[cpu, runID]}");
    }
    void FinishGraphRuntimeData()
    {
        string table = SaveUtility.CreateCSV(graphRuntimeData);
        Debug.Log(table);
        SaveUtility.CreateFile(table, "CSVTest1.csv");
    }
    private void Start()
    {
        graphRuntimeData = new string[cpuDebugs.Length, GraphBuildsMax];
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

        //for (int i = 0; i < 11; i += 1)
        //{
        //    WorldBuilder.BuiltWorlds.RemoveAt(10);
        //}

        //foreach(World world in WorldsSource.BuiltWorlds)
        //{
        //    if (world.name.Contains("7/29/21"))
        //    {
        //        WorldBuilder.AddWorld(world);
        //        WorldsSource.BuiltWorlds.Remove(world);
        //    }
        //}
        //WorldBuilder.BuiltWorlds.Add( WorldsSource.BuiltWorlds[WorldsSource.BuiltWorlds.Count - 1]);
        //WorldsSource.BuiltWorlds.RemoveAt(WorldsSource.BuiltWorlds.Count - 1);

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
    private GameObject[,] pathPoints;
    bool testDone = false;

    
    public int GraphBuildsMax = 1;
    int graphBuildsCount = 0;
    double totalTime = 0;
    private void Update()
    {
        if (MapSource == MapSources.History)
        {

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                historyIndex += 1;
                RoomHistory rh = historySource.WorldHistory.GetRoom(historyIndex);
                Room room = worldHistory.GetRoom(rh.roomID);
                if (rh.destroyed)
                {
                    Debug.Log("RemoveMap");
                    FindObjectOfType<BuildWorld>().HideRoom(room);
                    //Generator.RemoveMap(room);
                    RemovePath(room);
                }
                else
                {
                    room.SetupRoom(rh.map);
                    FindObjectOfType<BuildWorld>().DisplayRoom(room);
                    //Generator.ConvertMap(room);
                    AddPath(room);
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
                    FindObjectOfType<BuildWorld>().HideRoom(room);
                    //Generator.RemoveMap(room);
                    RemovePath(room);
                }
                else
                {
                    Room room = worldHistory.GetRoom(rh.roomID);
                    FindObjectOfType<BuildWorld>().DisplayRoom(room);
                    //Generator.ConvertMap(room);
                    AddPath(room);
                }

                historyIndex -= 1;

                RoomID.text = historySource.WorldHistory.GetRoom(historyIndex).roomID.ToString();
                BuildTime.text = Utility.FormatTime(historySource.WorldHistory.GetRoom(historyIndex).buildTime);
                RunNum.text = (historyIndex + 1).ToString();

            }

        }
        else if (MapSource == MapSources.Graph && Solver.SolverStatus == ClingoSolver.Status.SATISFIABLE && !testDone)
        {
            Graph worldGraph = WorldMap.ConvertGraph(Solver.answerSet);
            Transform miniMap = Instantiate(MiniMap, MiniMap.transform.position + new Vector3(graphBuildsCount * worldWidth, - cpuDebugsIndex * worldHeight, 0), Quaternion.identity).transform;
            WorldMap.DisplayGraph(worldGraph, nodePrefab, edgePrefab, miniMap);
            GraphRuntimeData((float)Solver.Duration, graphBuildsCount, cpuDebugsIndex, 0,0,0);
            Debug.Log($"------------------------number {graphBuildsCount + 1} : SATISFIABLE : {Solver.Duration} seconds-------------------------");
            totalTime += Solver.Duration;
            graphBuildsCount += 1;
            if (graphBuildsCount < GraphBuildsMax)
            {
                FindObjectOfType<BuildWorld>().BuildGraph(worldWidth, worldHeight, keyTypeCount, maxGatePerKey, minGatePerKey, bossGateKey, 3, timeout, cpuDebugs[cpuDebugsIndex]);
            }
            else
            {
                Debug.Log($"------------------------all {graphBuildsCount} : COMPLETE : {totalTime} seconds-------------------------");
                if (cpuDebugsIndex < cpuDebugs.Length-1)
                {
                    graphBuildsCount = 0;
                    cpuDebugsIndex += 1;
                }
                else
                {
                    FinishGraphRuntimeData();
                    testDone = true;
                }
                
            }

            //timeout -= 100;
        }
        else if (MapSource == MapSources.Graph && Solver.SolverStatus == ClingoSolver.Status.TIMEDOUT && !testDone)
        {
            timeout += 100;
            Debug.Log($"------------------------number {graphBuildsCount + 1} : TIMEDOUT : {timeout} seconds-------------------------");
            totalTime += timeout;
            FindObjectOfType<BuildWorld>().BuildGraph(worldWidth, worldHeight, keyTypeCount, maxGatePerKey, minGatePerKey, bossGateKey, 3, timeout, cpus);
        }
    }
    public void AddPath(Room room)
    {
        foreach(WorldBuilder.Path path in room.map.paths)
        {
            int width = room.map.dimensions.room_width;
            int height = room.map.dimensions.room_height;
            int i = room.pos.x;
            int j = room.pos.y;
            int x = path.x + width * i;
            int y = path.y + height * j;
            
            string type = path.type;

            if(pathPoints == null) pathPoints = new GameObject[width * worldWidth + 2, height * worldHeight + 2];

            if (!pathPoints[x, y])
            {

                PathNode node = Instantiate(pathPrefab).GetComponent<PathNode>();
                pathPoints[x, y] = node.gameObject;
                node.SetUpPathNode(x , -(y), type);
            }
            else
            {
                pathPoints[x, y].GetComponent<PathNode>().AddNode(type);
            }
        }
    }
    public void RemovePath(Room room)
    {
        foreach (WorldBuilder.Path path in room.map.paths)
        {
            int width = room.map.dimensions.room_width;
            int height = room.map.dimensions.room_height;
            int i = room.pos.x;
            int j = room.pos.y;
            int x = path.x + width * i;
            int y = path.y + height * j;
            string type = path.type;
            if (pathPoints[x, y])
            {
                Destroy(pathPoints[x,y]);
                pathPoints[x, y] = null;
            }
            else
            {

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
            FindObjectOfType<BuildWorld>().Worlds = WorldBuilder;
            FindObjectOfType<BuildWorld>().BuildAWorld(worldWidth, worldHeight, keyTypeCount, maxGatePerKey, minGatePerKey,bossGateKey, RoomSize.x, RoomSize.y, headroom, shoulderroom, jumpHeadroom, timeout, cpus);
        }
        else if (MapSource == MapSources.World)
        {
            //WorldBuilder.BuildWorld(worldWidth, worldHeight, keyTypeCount, maxGatePerKey, 3, Solver.maxDuration - 10);
            World world = WorldBuilder.BuiltWorlds[builtWorldIndex];
            //WorldMap.ConvertGraph()
            foreach(Room room in world.GetRooms())
            {
                room.SetupRoom();
                FindObjectOfType<BuildWorld>().DisplayRoom(room);
                //Generator.ConvertMap(room);
            }
        }else if(MapSource == MapSources.History)
        {


            historySource = WorldBuilder.BuiltWorlds[builtWorldIndex];

            WorldMap.DisplayGraph(historySource.worldGraph, nodePrefab, edgePrefab, MiniMap.transform);
            

            worldHistory = new World(historySource.Width, historySource.Height);
            historyIndex = 0;
            RoomHistory rh = historySource.WorldHistory.GetRoom(historyIndex);
            Room room = worldHistory.GetRoom(rh.roomID);
            room.SetupRoom(rh.map);
            FindObjectOfType<BuildWorld>().DisplayRoom(room);
            //Generator.ConvertMap(room);
            

            TotalTime.text = Utility.FormatTime(historySource.WorldHistory.GetTotalTime());
            RoomID.text = rh.roomID.ToString();
            BuildTime.text = ((int)rh.buildTime).ToString();
            RunNum.text = (historyIndex + 1).ToString();

            historySource.WorldHistory.GetRoomHistoryAnalysis();

            int width = historySource.Width;
            int roomWidth = room.map.dimensions.room_width;
            int height = historySource.Height;
            int roomHeight = room.map.dimensions.room_height;
            pathPoints = new GameObject[width * roomWidth + 2, height * roomHeight + 2];
            AddPath(room);
        }else if(MapSource == MapSources.Graph)
        {
            FindObjectOfType<BuildWorld>().BuildGraph(worldWidth, worldHeight, keyTypeCount, maxGatePerKey, minGatePerKey, bossGateKey, 3, timeout, cpuDebugs[0]);
        }


    }
    
    
}



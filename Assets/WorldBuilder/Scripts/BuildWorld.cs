using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    public class BuildWorld : MonoBehaviour
    {
        public MapGenerator Generator;
        public Worlds WorldBuilder;
        [SerializeField] private World world;
        public ClingoSolver Solver;
        public bool SolvingWorld;
        public GameObject nodePrefab, edgePrefab;
        public int worldWidth, worldHeight, keyCount, maxGatePerKey, roomWidth, roomHeight, headroom, shoulderroom, jumpHeadroom, timeout;

        public enum BuildStates
        {
            idle,
            graph,
            graphBuilding,
            roomBuildingSetup,
            roomBuilding,
            roomBuilt,
            solved,
            complete
        }
        public BuildStates BuildState;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (BuildState != BuildStates.complete && BuildState != BuildStates.idle && Solver.SolverStatus != ClingoSolver.Status.RUNNING)
            {
                BuildingWorld();
            }
        }

        public void BuildAWorld(int worldWidth, int worldHeight, int keyCount, int maxGatePerKey, int roomWidth, int roomHeight, int headroom, int shoulderroom, int jumpHeadroom, int timeout)
        {
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;
            this.keyCount = keyCount;
            this.maxGatePerKey = maxGatePerKey;
            this.roomWidth = roomWidth;
            this.roomHeight = roomHeight;
            this.headroom = headroom;
            this.shoulderroom = shoulderroom;
            this.jumpHeadroom = jumpHeadroom;
            this.timeout = timeout;
            BuildState = BuildStates.graph;
            world = new World(worldWidth, worldHeight);
            world.name = $"World {worldWidth}x{worldHeight} Rooms {roomWidth}x{roomHeight}";
        }

        void BuildingWorld()
        {
            switch (BuildState)
            {
                case BuildStates.graph:
                    WorldBuilder.BuildWorld(worldWidth, worldHeight, keyCount, maxGatePerKey, 3, timeout);
                    BuildState += 1;
                    break;
                case BuildStates.graphBuilding:
                    if (Solver.SolverStatus == ClingoSolver.Status.SATISFIABLE)
                    {
                        world.worldGraph = WorldMap.ConvertGraph(Solver.answerSet);
                        world.rawGraph = Solver.answerSet;
                        WorldMap.DisplayGraph(Solver.answerSet, nodePrefab, edgePrefab);
                        BuildState += 1;
                    }
                    else
                    {
                        BuildState = BuildStates.graph;
                    }

                    break;
                case BuildStates.roomBuildingSetup:
                    BuildingRoomsSetup();
                    break;
                case BuildStates.roomBuilding:
                    BuildingRooms();
                    break;
                case BuildStates.roomBuilt:
                    BuiltRoom();
                    break;
                case BuildStates.solved:
                    //handle object clean up
                    WorldBuilder.AddWorld(world);
                    BuildState += 1;
                    break;
                case BuildStates.complete:
                    //ready for next job
                    break;
                default:
                    Debug.LogWarning($"BuildingWorld case not handled: {BuildState}");
                    break;

            }

        }

        List<Vector2Int> BuildQueue;
        RoomConnections[] connections;

        void BuildingRoomsSetup()
        {
            connections = WorldMap.get_room_connections(world.rawGraph);
            BuildQueue = new List<Vector2Int>();
            for(int y = 0; y < worldHeight; y += 1)
            {
                for(int x = 0; x < worldWidth; x += 1)
                {
                    BuildQueue.Add(new Vector2Int(x, y));
                }
            }
            BuildState += 1;
        }

        void BuildingRooms()
        {
            if(BuildQueue.Count > 0)
            {
                Vector2Int index = BuildQueue[0];
                BuildQueue.RemoveAt(0);
                BuildARoom(index);
                BuildState += 1;
            }
            else
            {
                BuildState = BuildStates.solved;
            }
        }

        Vector2Int currentRoom;
        void BuildARoom(Vector2Int index)
        {
            currentRoom = index;
            int roomID = Utility.index_to_roomID(index,worldWidth,worldHeight);

            Debug.Log("Building roomID: " + roomID + " index: " + index);
            Neighbors neighbors = world.GetNeighbors(roomID);
            //bool[] connection = WorldMap.GetConnections(connections[roomID]);
            WorldBuilder.BuildRoom(new Vector2Int(roomWidth, roomHeight), headroom, shoulderroom, jumpHeadroom, connections[roomID], neighbors);
            //BuildState += 1;
        }

        public bool display = true;
        void BuiltRoom()
        {
            int roomID = Utility.index_to_roomID(currentRoom, worldWidth, worldHeight);
            if (Solver.SolverStatus == ClingoSolver.Status.SATISFIABLE)
            {
                
                Debug.Log(roomID);
                Room newRoom = world.GetRoom(roomID);
                newRoom.SetupRoom(Solver.answerSet);
                if (display) Generator.ConvertMap(newRoom);
                BuildState = BuildStates.roomBuilding;
            }else if(Solver.SolverStatus == ClingoSolver.Status.UNSATISFIABLE)
            {
                Room killRoom = world.GetRandomNeighbor(roomID);
                killRoom.isDestroyed = true;
                if (display) Generator.RemoveMap(killRoom);
                BuildQueue.Insert(0, killRoom.pos);
                BuildQueue.Insert(0, currentRoom);
                BuildState = BuildStates.roomBuilding;
                Debug.Log(Solver.SolverStatus + ": removing roomID: " + Utility.index_to_roomID(killRoom.pos, worldWidth, worldHeight) + " index: " + killRoom.pos);
            }
            else if (Solver.SolverStatus == ClingoSolver.Status.TIMEDOUT)
            {
                Room killRoom = world.GetRandomNeighbor(roomID);
                killRoom.isDestroyed = true;
                if (display) Generator.RemoveMap(killRoom);
                BuildQueue.Insert(0, killRoom.pos);
                BuildQueue.Insert(0, currentRoom);
                BuildState = BuildStates.roomBuilding;
                Debug.Log(Solver.SolverStatus + ": removing roomID: " + Utility.index_to_roomID(killRoom.pos, worldWidth, worldHeight) + " index: " + killRoom.pos);
            }
            else
            {
                BuildState = BuildStates.solved;
                Debug.LogWarning("Unhandled ClingoSolver.Status");
            }
        }

        //public Room GetRandomNeighbor(int roomID)
        //{
        //    Neighbors neighbors = world.GetNeighbors(roomID);
        //    return neighbors.RandomNeighbor();
        //}
    }
}
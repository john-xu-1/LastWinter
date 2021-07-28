using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Clingo;
namespace WorldBuilder
{
    public class BuildWorld : MonoBehaviour
    {
        public MapGenerator Generator;
        public Worlds Worlds;
        [SerializeField] private World world;
        public ClingoSolver Solver;
        public bool SolvingWorld;
        public GameObject nodePrefab, edgePrefab;
        public int worldWidth, worldHeight, keyCount, maxGatePerKey, roomWidth, roomHeight, headroom, shoulderroom, jumpHeadroom, timeout;
        public GameObject MiniMap;

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
                    BuildGraph(worldWidth, worldHeight, keyCount, maxGatePerKey, 3, timeout);
                    BuildState += 1;
                    break;
                case BuildStates.graphBuilding:
                    if (Solver.SolverStatus == ClingoSolver.Status.SATISFIABLE)
                    {
                        world.worldGraph = WorldMap.ConvertGraph(Solver.answerSet);
                        world.rawGraph = Solver.answerSet;
                        WorldMap.DisplayGraph(Solver.answerSet, nodePrefab, edgePrefab, MiniMap.transform);
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
                    world.WorldState = World.WorldStates.Built;
                    Worlds.AddWorld(world);
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
            BuildRoom(new Vector2Int(roomWidth, roomHeight), headroom, shoulderroom, jumpHeadroom, connections[roomID], neighbors);
            //BuildState += 1;
        }

        public bool display = true;
        public bool history = true;
        void BuiltRoom()
        {
            int roomID = Utility.index_to_roomID(currentRoom, worldWidth, worldHeight);
            if (Solver.SolverStatus == ClingoSolver.Status.SATISFIABLE)
            {
                double buildTime = Solver.Duration;//Time.fixedTime - buildTimeStart;
                //Debug.Log(roomID);
                Room newRoom = world.GetRoom(roomID);
                newRoom.SetupRoom(Solver.answerSet);

                //remove correct permutation
                if(newRoom.buidStatus == ClingoSolver.Status.UNSATISFIABLE)
                {
                    List<int> killRooms = newRoom.removedNeighbors;
                    foreach(int killID in killRooms)
                    {
                        Room killRoom = world.GetRoom(killID);
                        DestroyRoom(killRoom,newRoom.lastBuildTime, roomID, ClingoSolver.Status.UNSATISFIABLE);
                    }
                }

                if (history) world.WorldHistoryAdd(roomID, newRoom.map, buildTime, Solver.SolverStatus);
                if (display) Generator.ConvertMap(newRoom);

                newRoom.buidStatus = ClingoSolver.Status.SATISFIABLE;
                BuildState = BuildStates.roomBuilding;
            }else if(Solver.SolverStatus == ClingoSolver.Status.UNSATISFIABLE)
            {
                double destroyTime = Solver.Duration; // Time.fixedTime - buildTimeStart;

                Room room = world.GetRoom(roomID);

                if(room.buidStatus != ClingoSolver.Status.UNSATISFIABLE)
                {
                    List<List<int>> indices = Utility.GetPermutations(world.GetNeighborIDs(roomID));
                    room.neighborPermutations = indices;
                    room.removedNeighbors = Utility.GetSmallestRandomPermutation(indices, true);
                }
                else
                {
                    room.removedNeighbors = Utility.GetSmallestRandomPermutation(room.neighborPermutations, true);
                    
                }

                //Room killRoom = world.GetRandomNeighbor(roomID);
                //killRoom.isDestroyed = true;
                //if (history) {
                //    int killRoomID = Utility.index_to_roomID(killRoom.pos, worldWidth, worldHeight);
                //    world.WorldHistoryRemove(killRoomID, killRoom.map, destroyTime, roomID, Solver.SolverStatus);

                //}
                //if (display) Generator.RemoveMap(killRoom);
                //BuildQueue.Insert(0, killRoom.pos);
                BuildQueue.Insert(0, currentRoom);

                if(room.buidStatus != ClingoSolver.Status.UNSATISFIABLE)
                {
                    room.buidStatus = ClingoSolver.Status.UNSATISFIABLE;
                    room.lastBuildTime = destroyTime;
                }

                string list = "";
                foreach(int id in room.removedNeighbors)
                {
                    list += $"roomID: {id} ";
                }
                Debug.Log($"{Solver.SolverStatus}: Checking {list} for SATIFIABILITY");

                BuildState = BuildStates.roomBuilding;
                //Debug.Log(Solver.SolverStatus + ": removing roomID: " + Utility.index_to_roomID(killRoom.pos, worldWidth, worldHeight) + " index: " + killRoom.pos);
            }
            else if (Solver.SolverStatus == ClingoSolver.Status.TIMEDOUT)
            {

                Room killRoom = world.GetRandomNeighbor(roomID);
                if(killRoom != null)
                {
                    double destroyTime = Solver.Duration;// Time.fixedTime - buildTimeStart;
                    //killRoom.isDestroyed = true;
                    //if (history)
                    //{
                    //    float destroyTime = Time.fixedTime - buildTimeStart;
                    //    int killRoomID = Utility.index_to_roomID(killRoom.pos, worldWidth, worldHeight);
                    //    world.WorldHistoryRemove(killRoomID, killRoom.map, destroyTime, roomID, Solver.SolverStatus);

                    //}
                    //if (display) Generator.RemoveMap(killRoom);
                    DestroyRoom(killRoom, destroyTime, roomID, Solver.SolverStatus);
                    //BuildQueue.Insert(0, killRoom.pos);
                    //Debug.Log(Solver.SolverStatus + ": removing roomID: " + Utility.index_to_roomID(killRoom.pos, worldWidth, worldHeight) + " index: " + killRoom.pos);
                }
                else
                {
                    Debug.Log(Solver.SolverStatus + ": has no neighbors");
                }
                
                BuildQueue.Insert(0, currentRoom);

                world.GetRoom(roomID).buidStatus = ClingoSolver.Status.TIMEDOUT;
                BuildState = BuildStates.roomBuilding;
                
            }
            else
            {
                BuildState = BuildStates.solved;
                Debug.LogWarning("Unhandled ClingoSolver.Status");
            }
        }
        void DestroyRoom(Room killRoom, double destroyTime, int destoryerID, Clingo.ClingoSolver.Status status)
        {
            killRoom.isDestroyed = true;
            if (history) world.WorldHistoryRemove(killRoom, destroyTime, destoryerID, status);
            if(display) Generator.RemoveMap(killRoom);
            BuildQueue.Insert(0, killRoom.pos);
            Debug.Log(status + ": removing roomID: " + Utility.index_to_roomID(killRoom.pos, worldWidth, worldHeight) + " index: " + killRoom.pos);
        }

        void BuildGraph(int worldWidth, int worldHeight, int gateKeyCount, int maxGatePerKey, int startRoom, int timeout)
        {
            string aspCode = WorldMap.bidirectional_rules + WorldMap.test_text + WorldMap.gate_key_rules;
            string path = ClingoUtil.CreateFile(aspCode);
            ClingoSolver solver = FindObjectOfType<ClingoSolver>();
            solver.Solve(path, $" -c max_width={worldWidth} -c max_height={worldHeight} -c start_room={startRoom} -c key_count={gateKeyCount} -c max_gate_type_count={maxGatePerKey}  -t 1 --time-limit={timeout}");
        }

        float buildTimeStart = 0;
        void BuildRoom(Vector2Int roomSize, int headroom, int shoulderroom, int minCeilingHeight, RoomConnections connections, Neighbors neighbors)
        {

            //Debug.Log(WorldStructure.max_width + " " + WorldStructure.max_height);
            string aspCode = WorldStructure.get_world_gen(roomSize.x, roomSize.y) + WorldStructure.tile_rules + WorldStructure.get_floor_rules(headroom, shoulderroom) + WorldStructure.get_chamber_rule(minCeilingHeight) + Pathfinding.movement_rules + Pathfinding.platform_rules + Pathfinding.path_rules;

            aspCode += Pathfinding.set_openings(connections.boolArray) + WorldStructure.GetDoorRules(neighbors);

            if ((connections.leftEgress || connections.leftIngress) && neighbors.left != null && !neighbors.left.isDestroyed)
            {
                //Debug.Log(Pathfinding.getPathStartRules("left", neighbors.left));
                aspCode += Pathfinding.getPathStartRules("left", neighbors.left);
            }
            if ((connections.rightEgress || connections.rightIngress) && neighbors.right != null && !neighbors.right.isDestroyed)
            {
                //Debug.Log(Pathfinding.getPathStartRules("right", neighbors.right));
                aspCode += Pathfinding.getPathStartRules("right", neighbors.right);
            }
            if ((connections.upEgress || connections.upIngress) && neighbors.up != null && !neighbors.up.isDestroyed)
            {
                //Debug.Log(Pathfinding.getPathStartRules("top", neighbors.up));
                aspCode += Pathfinding.getPathStartRules("top", neighbors.up);
            }
            if ((connections.downEgress || connections.downIngress) && neighbors.down != null && !neighbors.down.isDestroyed)
            {
                //Debug.Log(Pathfinding.getPathStartRules("bottom", neighbors.down));
                aspCode += Pathfinding.getPathStartRules("bottom", neighbors.down);
            }
            ClingoSolver solver = FindObjectOfType<ClingoSolver>();

            string path = ClingoUtil.CreateFile(aspCode);
            buildTimeStart = Time.fixedTime;
            solver.Solve(path, " --parallel-mode 4 ");
        }
    }
}
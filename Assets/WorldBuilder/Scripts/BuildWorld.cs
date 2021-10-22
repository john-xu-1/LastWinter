using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Clingo;
namespace WorldBuilder
{
    public class BuildWorld : MonoBehaviour
    {
        //public bool DebugMode { get { return debugMode; } }
        [SerializeField] private bool debugMode;
        public bool DebugMode(Debugging.Debugger.DebugTypes source) { if (FindObjectOfType<Debugging.Debugger>()) return FindObjectOfType<Debugging.Debugger>().Debug(source); else return debugMode; }
        public MapGenerator Generator;
        public FreeObjects FreeObjects;
        public Worlds Worlds;
        [SerializeField] private World world;
        public ClingoSolver Solver;
        public bool SolvingWorld;
        public GameObject nodePrefab, edgePrefab;
        public int worldWidth, worldHeight, keyCount, maxGatePerKey, minGatePerKey, bossGateKey, roomWidth, roomHeight, headroom, shoulderroom, jumpHeadroom, timeout, cpus;
        GateTypes[] gates;
        public GameObject MiniMap;
        public GameObject KeyPrefab;

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

        public void BuildAWorld(int worldWidth, int worldHeight, int keyCount, int maxGatePerKey, int minGatePerKey, int bossGateKey, int roomWidth, int roomHeight, int headroom, int shoulderroom, int jumpHeadroom, int timeout,int cpus, GateTypes[] gates)
        {
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;
            this.keyCount = keyCount;
            this.maxGatePerKey = maxGatePerKey;
            this.minGatePerKey = minGatePerKey;
            this.bossGateKey = bossGateKey;
            this.roomWidth = roomWidth;
            this.roomHeight = roomHeight;
            this.headroom = headroom;
            this.shoulderroom = shoulderroom;
            this.jumpHeadroom = jumpHeadroom;
            this.timeout = timeout;
            this.cpus = cpus;
            this.gates = gates;
            BuildState = BuildStates.graph;
            world = new World(worldWidth, worldHeight);
            world.name = $"World {worldWidth}x{worldHeight} Rooms {roomWidth}x{roomHeight}";

            if (DebugMode(Debugging.Debugger.DebugTypes.debugData) && FindObjectOfType<Debugging.DebugData>()) FindObjectOfType<Debugging.DebugData>().RuntimeDataStart(22);
        }

        void BuildingWorld()
        {
            switch (BuildState)
            {
                case BuildStates.graph:
                    BuildGraph(worldWidth, worldHeight, keyCount, maxGatePerKey, minGatePerKey, bossGateKey, 3, timeout,cpus);
                    BuildState += 1;
                    break;
                case BuildStates.graphBuilding:
                    if (Solver.SolverStatus == ClingoSolver.Status.SATISFIABLE)
                    {
                        world.worldGraph = WorldMap.ConvertGraph(Solver.answerSet);
                        world.rawGraph = Solver.answerSet;
                        WorldMap.DisplayGraph(world.worldGraph, nodePrefab, edgePrefab, MiniMap.transform);
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
                    
                    world.FinishWorldBuild();
                    Worlds.AddWorld(world);
                    
                    if (DebugMode(Debugging.Debugger.DebugTypes.debugData))
                    {
                        FindObjectOfType<Debugging.DebugData>().FinishRuntimeData();
                    }
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

        public List<Vector2Int> BuildQueue;
        RoomConnections[] connections;

        void BuildingRoomsSetup()
        {
            connections = WorldMap.get_room_connections(world.rawGraph);
            BuildQueue = new List<Vector2Int>();
            foreach (Key key in world.worldGraph.keys)
            {
                Vector2Int keyRoom = Utility.roomID_to_index(key.roomID, worldWidth, worldHeight);
                BuildQueue.Add(keyRoom);
            }
            foreach (Gate gate in world.worldGraph.gates)
            {
                Vector2Int gateRoom = Utility.roomID_to_index(gate.source, worldWidth, worldHeight);
                BuildQueue.Add(gateRoom);
            }
            for (int y = 0; y < worldHeight; y += 1)
            {
                for(int x = 0; x < worldWidth; x += 1)
                {
                    Vector2Int newRoom = new Vector2Int(x, y);
                    if(!BuildQueue.Contains(newRoom)) BuildQueue.Add(newRoom);
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
            Gate gate = Gates.GetGate(world.worldGraph, roomID);
            Gated gated = Gates.GetGated(world.worldGraph, roomID);
            Key key = KeyFreeObject.GetKey(world.worldGraph, roomID);
            BuildRoom(gate,gated,key, roomID, new Vector2Int(roomWidth, roomHeight), headroom, shoulderroom, jumpHeadroom, cpus, connections[roomID], neighbors, gates);
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
                if (world.worldGraph.startRoomID == roomID) newRoom.startRoom = true;
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

                if (history) world.WorldHistoryAdd(roomID, newRoom.map, newRoom.items, buildTime, Solver.SolverStatus);
                if (display) DisplayRoom(newRoom);

                if (DebugMode(Debugging.Debugger.DebugTypes.debugData)) {
                    Neighbors neighbors = world.GetNeighbors(roomID);
                    Gate gate = Gates.GetGate(world.worldGraph, roomID);
                    Gated gated = Gates.GetGated(world.worldGraph, roomID);
                    Key key = KeyFreeObject.GetKey(world.worldGraph, roomID);
                    Debugging.DebugRoom debugRoom = new Debugging.DebugRoom(gate, gated,key, new Vector2Int(roomWidth, roomHeight),headroom,shoulderroom,jumpHeadroom,cpus,connections[roomID],gates,neighbors);
                    FindObjectOfType<Debugging.DebugData>().RoomRuntimeData(roomID,(float)Solver.Duration, debugRoom, newRoom, Solver.SolverStatus);
                }

                newRoom.buidStatus = ClingoSolver.Status.SATISFIABLE;
                BuildState = BuildStates.roomBuilding;
                NonTimeout();
            }else if(Solver.SolverStatus == ClingoSolver.Status.UNSATISFIABLE)
            {
                double destroyTime = Solver.Duration; // Time.fixedTime - buildTimeStart;

                Room room = world.GetRoom(roomID);

                if (DebugMode(Debugging.Debugger.DebugTypes.debugData))
                {
                    Neighbors neighbors = world.GetNeighbors(roomID);
                    Gate gate = Gates.GetGate(world.worldGraph, roomID);
                    Gated gated = Gates.GetGated(world.worldGraph, roomID);
                    Key key = KeyFreeObject.GetKey(world.worldGraph, roomID);
                    Debugging.DebugRoom debugRoom = new Debugging.DebugRoom(gate, gated, key, new Vector2Int(roomWidth, roomHeight), headroom, shoulderroom, jumpHeadroom, cpus, connections[roomID], gates, neighbors);
                    FindObjectOfType<Debugging.DebugData>().RoomRuntimeData(roomID, 0, debugRoom, room, Solver.SolverStatus);
                }

                if (room.buidStatus != ClingoSolver.Status.UNSATISFIABLE)
                {
                    List<List<int>> indices = Utility.GetPermutations(world.GetNeighborIDs(roomID));
                    room.neighborPermutations = indices;
                    room.removedNeighbors = Utility.GetSmallestRandomPermutation(indices, true);
                }
                else
                {
                    room.removedNeighbors = Utility.GetSmallestRandomPermutation(room.neighborPermutations, true);
                    
                }

                
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
                world.GetRoom(roomID).buidStatus = ClingoSolver.Status.TIMEDOUT;

                if (DebugMode(Debugging.Debugger.DebugTypes.debugData))
                {
                    Room room = world.GetRoom(roomID);
                    Neighbors neighbors = world.GetNeighbors(roomID);
                    Gate gate = Gates.GetGate(world.worldGraph, roomID);
                    Gated gated = Gates.GetGated(world.worldGraph, roomID);
                    Key key = KeyFreeObject.GetKey(world.worldGraph, roomID);
                    Debugging.DebugRoom debugRoom = new Debugging.DebugRoom(gate, gated, key, new Vector2Int(roomWidth, roomHeight), headroom, shoulderroom, jumpHeadroom, cpus, connections[roomID], gates, neighbors);
                    FindObjectOfType<Debugging.DebugData>().RoomRuntimeData(roomID, Solver.maxDuration - 10, debugRoom, room, Solver.SolverStatus);
                }

                Room killRoom = world.GetRandomNeighbor(roomID);
                if(killRoom != null)
                {
                    double destroyTime = Solver.Duration;// Time.fixedTime - buildTimeStart;
                   
                    DestroyRoom(killRoom, destroyTime, roomID, Solver.SolverStatus);
                    
                }
                else
                {
                    Debug.Log(Solver.SolverStatus + ": has no neighbors");
                }
                
                BuildQueue.Insert(0, currentRoom);

                
                BuildState = BuildStates.roomBuilding;

                Timedout();
            }
            else
            {
                BuildState = BuildStates.complete;
                Debug.LogWarning($"Unhandled ClingoSolver.Status: {Solver.SolverStatus}");
            }
        }
        public void DisplayRoom(Room room)
        {
            //Generator.ConvertMap(room);
            //room.BuildRoom(FreeObjects);
            Generator.BuildRoom(room);
            if (FindObjectOfType<Debugging.DebugMap>()) FindObjectOfType<Debugging.DebugMap>().AddPath(room);
        }
        public void HideRoom(Room room)
        {
            Generator.RemoveMap(room);
            if (FindObjectOfType<Debugging.DebugMap>()) FindObjectOfType<Debugging.DebugMap>().RemovePath(room);
        }
        void DestroyRoom(Room killRoom, double destroyTime, int destoryerID, Clingo.ClingoSolver.Status status)
        {
            killRoom.isDestroyed = true;
            if (history) world.WorldHistoryRemove(killRoom, destroyTime, destoryerID, status);
            if (display) HideRoom(killRoom);
            BuildQueue.Insert(0, killRoom.pos);
            Debug.Log(status + ": removing roomID: " + Utility.index_to_roomID(killRoom.pos, worldWidth, worldHeight) + " index: " + killRoom.pos);
        }

        public void BuildGraph(int worldWidth, int worldHeight, int gateKeyCount, int maxGatePerKey, int minGatePerKey, int bossGateKey, int startRoom, int timeout, int cpus)
        {
            string aspCode = WorldMap.bidirectional_rules + WorldMap.test_text + WorldMap.gate_key_rules;
            string path = ClingoUtil.CreateFile(aspCode);
            ClingoSolver solver = FindObjectOfType<ClingoSolver>();
            solver.maxDuration = timeout + 10;
            solver.Solve(path, $" -c max_width={worldWidth} -c max_height={worldHeight} -c start_room={startRoom} -c key_count={gateKeyCount} -c max_gate_type_count={maxGatePerKey} -c min_gate_type_count={minGatePerKey} -c boss_gate_type={bossGateKey} --parallel-mode {cpus} --time-limit={timeout}");
        }

        float buildTimeStart = 0;
        public void BuildRoom(Gate gate, Gated gated, Key key, int roomID,Vector2Int roomSize, int headroom, int shoulderroom, int minCeilingHeight, int cpus, RoomConnections connections, Neighbors neighbors, GateTypes[] gates)
        {

            //Debug.Log(WorldStructure.max_width + " " + WorldStructure.max_height);
            string aspCode = "";
            aspCode += WorldStructure.get_world_gen(roomSize.x, roomSize.y);
            if(!DebugMode(Debugging.Debugger.DebugTypes.tile_rules)) aspCode += WorldStructure.tile_rules;
            aspCode += WorldStructure.get_floor_rules(headroom, shoulderroom);
            aspCode += WorldStructure.get_chamber_rule(minCeilingHeight);
            aspCode += Pathfinding.movement_rules;
            aspCode += Pathfinding.platform_rules;
            aspCode += Pathfinding.path_rules;

            //GateTypes[] gates = { GateTypes.water, GateTypes.lava, GateTypes.door };
            GateTypes[,] keys = { { GateTypes.door, GateTypes.enemy }, { GateTypes.lava, GateTypes.none }, { GateTypes.water, GateTypes.none } };

            aspCode += Pathfinding.set_openings(connections.boolArray);
            aspCode += WorldStructure.GetDoorRules(neighbors);
            aspCode += Gates.GetGateASP(gate, gated, gates,connections);
            aspCode += KeyFreeObject.GetKeyRoomRules(key, gates);

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
            solver.maxDuration =  GetTimeout() + 10;
            solver.Solve(path, $" --parallel-mode {cpus} --time-limit={GetTimeout()}");
        }
        int addedTimeoutMax = 1000;
        int addedTimeout = 0;
        int addedTimeoutStep = 100;

        private int GetTimeout()
        {
            return timeout  + addedTimeout;
        }
        private void Timedout()
        {
            addedTimeout = Mathf.Clamp(addedTimeout + addedTimeoutStep, 0, addedTimeoutMax);
        }
        private void NonTimeout()
        {
            addedTimeout = Mathf.Clamp(addedTimeout - addedTimeoutStep, 0, addedTimeoutMax);
        }
    }
}
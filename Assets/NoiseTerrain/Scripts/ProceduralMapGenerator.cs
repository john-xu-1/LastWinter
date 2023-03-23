using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Threading;

namespace NoiseTerrain
{
    public class ProceduralMapGenerator : MapGenerator
    {
        public Vector2Int chunkID;
        public Vector2Int _chunkRadius = new Vector2Int(5, 4);
        public Vector2Int _tileRulesRadius = new Vector2Int(4, 3);
        public Vector2Int _tileRulesFixRadius = new Vector2Int(3, 2);
        public Vector2Int tileRadius = new Vector2Int(2, 1);

        private Vector2Int chunkRadius { get { return _chunkRadius + _tileRulesRadius + _tileRulesFixRadius; } }
        private Vector2Int tileRulesRadius { get { return _tileRulesRadius + _tileRulesFixRadius; } }
        private Vector2Int tileRulesFixRadius { get { return _tileRulesFixRadius; } }

        public List<Vector2Int> visibleChunkIDs = new List<Vector2Int>();
        public List<Vector2Int> toFixChunkIDs = new List<Vector2Int>();
        public List<Vector2Int> toDisplayChunks = new List<Vector2Int>();

        public Vector2Int roomSize = new Vector2Int(32, 32);
        public Transform target;


        public TileRules tileRules;
        public TileBase waterTile;
        public Tilemap waterTilemap;
        public TileBase lavaTile;
        public Tilemap lavaTilemap;

        List<Chunk> chunks = new List<Chunk>();
        //public bool debugFixTileRules;
        public bool fixTileRules;
        Thread handleFixTileRulesThread;
        public FixSubChunk fixSubChunk;
        public bool active = false;

        public int displayCountPerFrame = 1000;

        private bool displayPlatformGraph;
        List<PlatformChunkGraph> platformGraph;
        class PlatformChunkGraph
        {
            public PlatformChunk platform;
            public Color graphColor;
        }

        private void OnDestroy()
        {
            fixTileRules = false;
            fixSubChunk.fixTileRules = fixTileRules;
            handleFixTileRulesThread.Abort();
            Debug.Log("Exit");
        }
        private void Start()
        {

            handleFixTileRulesThread = new Thread(HandleFixTileRulesThread);
            handleFixTileRulesThread.Start();
            //fixTileRules = true;
        }
        private void Update()
        {
            HandleMouseClickDebugging();
            if (!active) return;
            Vector2Int chunkID = GetChunkID(target.position);

            //debug platformGraph
            if (displayPlatformGraph)
            {
                foreach (PlatformChunkGraph platform in platformGraph)
                {
                    foreach (int nodeID in platform.platform.connectedPlatforms)
                    {
                        PlatformChunk destination = roomChunk.GetPlatform(nodeID);
                        Vector2 start = platform.platform.GetTilePos(platform.platform.groundTiles[0]);
                        Vector2 dir = destination.GetTilePos(destination.groundTiles[0]) - start;
                        Debug.DrawRay(start, dir, platform.graphColor);
                        //Debug.Log($"{platform.graphColor}");
                    }
                }
            }

            //update changed chunks
            for (int i = 0; i < visibleChunkIDs.Count; i += 1)
            {
                Chunk visibleChunk = GetChunk(visibleChunkIDs[i]);
                if (visibleChunk.valueChanged)
                {
                    if (!toDisplayChunks.Contains(visibleChunkIDs[i]))
                    {
                        //toDisplayChunks.Add(visibleChunkIDs[i]);
                        GenerateMap(visibleChunkIDs[i]);
                    }

                    visibleChunk.valueChanged = false;
                }
            }

            // ---------------------------chunk.Load(distance)--------------------------------
            //if(chunkID != this.chunkID)
            //{
            //    foreach(Vector2Int visibleChunkID in visibleChunkIDs)
            //    {
            //        int distance = (int)Vector2Int.Distance(visibleChunkID, chunkID);
            //        GetChunk(visibleChunkID).Load(distance);
            //    }
            //}

            //----------- display chunk -----------------------
            //for (int i = toDisplayChunks.Count - 1; i >= 0; i -= 1)
            //{
            //    lock (toFixChunkIDs)
            //    {
            //        if (!toFixChunkIDs.Contains(toDisplayChunks[i]))
            //        {
            //            //Debug.Log($"Displaying chunk {toDisplayChunks[i]}");
            //            GenerateMap(toDisplayChunks[i]);
            //            visibleChunkIDs.Add(toDisplayChunks[i]);
            //            toDisplayChunks.RemoveAt(i);
            //        }
            //    }

            //}
            // ------------------------- new display chunk -------------------------------
            int displayCountPerFrame = this.displayCountPerFrame;
            int toDisplayIndex = toDisplayChunks.Count - 1;
            while (toDisplayIndex >= 0 && displayCountPerFrame > 0)
            {
                lock (toFixChunkIDs)
                {
                    if (!toFixChunkIDs.Contains(toDisplayChunks[toDisplayIndex]))
                    {
                        //Debug.Log($"Displaying chunk {toDisplayChunks[i]}");
                        GenerateMap(toDisplayChunks[toDisplayIndex]);
                        visibleChunkIDs.Add(toDisplayChunks[toDisplayIndex]);
                        toDisplayChunks.RemoveAt(toDisplayIndex);
                        displayCountPerFrame -= 1;
                    }
                }
                toDisplayIndex -= 1;
            }
            CheckMap(chunkID);

            if (!setupComplete && toDisplayChunks.Count == 0 && toFixChunkIDs.Count == 0)
            {
                setupComplete = true;
            }
        }
        public void SetupProceduralMapGenerator(Vector2Int chunkRadius, Vector2Int chunkSize, Vector2 pos, bool active)
        {
            this.active = active;
            tileRadius = chunkRadius;
            transform.position = (Vector3)pos + Vector3.forward * transform.position.z;
            width = chunkSize.x;
            height = chunkSize.y;
        }

        //public override void GenerateMap()
        //{

        //}

        protected override int GetMinX()
        {

            return (chunkID.x - tileRadius.x) * width;

        }

        protected override int GetMinY()
        {
            return (chunkID.y - tileRadius.y) * height;

        }

        protected override int GetMaxX()
        {
            return (chunkID.x + tileRadius.x) * width + width - 1;

        }

        protected override int GetMaxY()
        {
            return (chunkID.y + tileRadius.y) * height + height - 1;



        }
        public void SetTile(Vector2Int pos, bool value)
        {
            int x = ((pos.x % width) + width) % width;
            int y = ((-pos.y % height) + height) % height;
            Debug.Log($"{x} {y}");
            GetChunk(GetChunkID(pos)).SetTile(x, y, value);
        }

        public bool GetTile(Vector2Int pos)
        {
            int x = ((pos.x % width) + width) % width;
            int y = ((-pos.y % height) + height) % height;
            Debug.Log($"{x} {y}");
            return GetChunk(GetChunkID(pos)).GetTile(x, y);
        }
        public void AddChunk(Chunk chunk)
        {
            //chunks.Add(chunk);
            chunkDict[chunk.chunkID] = chunk;
        }
        Dictionary<Vector2Int, Chunk> chunkDict = new Dictionary<Vector2Int, Chunk>();
        public Chunk GetChunk(Vector2Int chunkID)
        {
            //foreach (Chunk chunk in chunks)
            //{
            //    if (chunk.chunkID == chunkID) return chunk;
            //}
            //return null;
            if (chunkDict.ContainsKey(chunkID)) return chunkDict[chunkID];
            else return null;
        }

        public Vector2Int GetChunkID(Vector2 pos)
        {
            int xOffset = pos.x < 0 ? -1 : 0;
            int yOffset = pos.y > 0 ? 1 : 0;

            return new Vector2Int(xOffset + ((int)pos.x - xOffset) / width, -yOffset - ((int)pos.y - yOffset) / height);
        }

        public void CheckMap(Vector2Int chunkID)
        {
            if (chunkID != this.chunkID)
            {
                Debug.Log("chunkID != this.chunkID");
                lock (toFixChunkIDs)
                {
                    this.chunkID = chunkID;
                    //layer 1 largest layer
                    List<Vector2Int> toBuildChunks = new List<Vector2Int>();
                    for (int x = -chunkRadius.x - tileRadius.x; x <= chunkRadius.x + tileRadius.x; x += 1)
                    {
                        for (int y = -chunkRadius.y - tileRadius.y; y <= chunkRadius.y + tileRadius.y; y += 1)
                        {

                            toBuildChunks.Add(chunkID + new Vector2Int(x, y));
                        }
                    }

                    //layer 2 second largest layer
                    List<Vector2Int> toCheckTileRulesChunks = new List<Vector2Int>();
                    for (int x = -tileRulesRadius.x - tileRadius.x; x <= tileRulesRadius.x + tileRadius.x; x += 1)
                    {
                        for (int y = -tileRulesRadius.y - tileRadius.y; y <= tileRulesRadius.y + tileRadius.y; y += 1)
                        {
                            toCheckTileRulesChunks.Add(chunkID + new Vector2Int(x, y));
                        }
                    }

                    //layer 3
                    List<Vector2Int> toFixTileRulesChunks = new List<Vector2Int>();
                    for (int x = 0; x <= tileRulesFixRadius.x + tileRadius.x; x += 1)
                    {
                        for (int y = 0; y <= tileRulesFixRadius.y + tileRadius.y; y += 1)
                        {
                            if (fixTileRules)
                            {
                                toFixTileRulesChunks.Add(chunkID + new Vector2Int(x, y));
                                if (x != 0) toFixTileRulesChunks.Add(chunkID + new Vector2Int(-x, y));
                                if (y != 0) toFixTileRulesChunks.Add(chunkID + new Vector2Int(x, -y));
                                if (x != 0 && y != 0) toFixTileRulesChunks.Add(chunkID + new Vector2Int(-x, -y));
                            }
                        }
                    }



                    // layer ?? visible layer smallest layer
                    List<Vector2Int> toDisplayChunks = new List<Vector2Int>();
                    for (int x = -tileRadius.x; x <= tileRadius.x; x += 1)
                    {
                        for (int y = -tileRadius.y; y <= tileRadius.y; y += 1)
                        {
                            toDisplayChunks.Add(chunkID + new Vector2Int(x, y));
                        }
                    }

                    //find chunks to be removed
                    for (int i = visibleChunkIDs.Count - 1; i >= 0; i -= 1)
                    {

                        if (!toDisplayChunks.Contains(visibleChunkIDs[i]) && !this.toDisplayChunks.Contains(visibleChunkIDs[i]))
                        {
                            //Debug.LogWarning($"Removing {visibleChunkIDs[i]}");
                            ClearMap(visibleChunkIDs[i]);
                            visibleChunkIDs.RemoveAt(i);

                        }
                        else if (!toDisplayChunks.Contains(visibleChunkIDs[i]))
                        {
                            //Debug.LogWarning($"Removing 2 {visibleChunkIDs[i]} {this.toDisplayChunks.Remove(visibleChunkIDs[i])}"); // must remove next line for debug
                            this.toDisplayChunks.Remove(visibleChunkIDs[i]);
                            visibleChunkIDs.RemoveAt(i);

                        }
                    }

                    //build chunks
                    for (int i = 0; i < toBuildChunks.Count; i += 1)
                    {
                        Chunk chunk = GetChunk(toBuildChunks[i]);
                        if (chunk == null)
                        {
                            int minX = toBuildChunks[i].x * width;
                            int maxX = (toBuildChunks[i].x + 1) * width - 1;
                            int minY = toBuildChunks[i].y * height;
                            int maxY = (toBuildChunks[i].y + 1) * height - 1;

                            float threshold = 0;

                            //if (toBuildChunks[i].x % roomSize.x == 0 ||  toBuildChunks[i].y % roomSize.y == 0)
                            //    threshold = -1;
                            chunk = new Chunk(toBuildChunks[i], GenerateBoolMap(minX, maxX, minY, maxY, threshold), this);
                            AddChunk(chunk);
                        }
                    }

                    //check tileRules
                    for (int i = 0; i < toCheckTileRulesChunks.Count; i += 1)
                    {
                        Chunk chunk = GetChunk(toCheckTileRulesChunks[i]);
                        Utility.CheckTileRules(chunk, tileRules);
                        // Debug.Log($"Checking Chunk {chunk.chunkID}");
                    }

                    //fix tileRules
                    for (int i = 0; i < toFixTileRulesChunks.Count; i += 1)
                    {
                        if (!toFixChunkIDs.Contains(toFixTileRulesChunks[i]) && !visibleChunkIDs.Contains(toFixTileRulesChunks[i]) && !this.toDisplayChunks.Contains(toFixTileRulesChunks[i]))
                        {
                            Chunk chunk = GetChunk(toFixTileRulesChunks[i]);
                            Utility.CheckTileRules(chunk, tileRules); // need to check in case invalid were fixed in an overlapping subchunk
                            if (chunk.hasInvalidTile)
                            {

                                toFixChunkIDs.Add(toFixTileRulesChunks[i]);

                            }
                        }

                    }

                    for (int i = 0; i < toDisplayChunks.Count; i += 1)
                    {
                        if (!this.toDisplayChunks.Contains(toDisplayChunks[i]) && !visibleChunkIDs.Contains(toDisplayChunks[i]))
                        {
                            this.toDisplayChunks.Add(toDisplayChunks[i]);
                        }
                    }

                }

                Utility.SortToChunkIDs(chunkID, toFixChunkIDs);

            }


        }

        RoomChunk roomChunk;
        public int jumpHeight = 6;
        public bool platformSetupComplete;
        public void SetRoomChunk()
        {
            platformSetupComplete = false;
            List<Chunk> roomChunks = new List<Chunk>();
            //roomSize should be double the tileRadius if all visible chunks should be in one room
            for (int x = -roomSize.x / 2; x <= roomSize.x / 2; x += 1)
            {
                for (int y = -roomSize.y / 2; y <= roomSize.y / 2; y += 1)
                {
                    roomChunks.Add(GetChunk(chunkID + new Vector2Int(x, y)));
                }
            }
            this.roomChunks = roomChunks;
            Thread thread = new Thread(SetRoomChunkThread);
            thread.Start();
        }
        public void SetRoomChunk(List<Chunk> roomChunks)
        {
            platformSetupComplete = false;
            setupComplete = false;
            //active = true;
            this.roomChunks = roomChunks;
            Thread thread = new Thread(SetRoomChunkThread);
            thread.Start();
        }
        List<Chunk> roomChunks;
        public void SetRoomChunkThread()
        {
            roomChunk = new RoomChunk(roomChunks, jumpHeight);
            platformSetupComplete = true;
        }

        public List<PlatformChunk> GetPlatforms()
        {
            List<PlatformChunk> platforms = new List<PlatformChunk>();
            foreach (FilledChunk filledChunk in roomChunk.filledChunks)
            {
                foreach (PlatformChunk platform in filledChunk.platforms)
                {
                    platforms.Add(platform);
                }
            }
            return platforms;
        }

        public override void GenerateMap(int seed)
        {
            this.seed = seed;
            setupComplete = false;
            active = true;
        }

        public override void GenerateMap()
        {
            //int minX = chunkID.x * width;
            //int maxX = (chunkID.x + 1) * width - 1;
            //int minY = chunkID.y * height;
            //int maxY = (chunkID.y + 1) * height - 1;
            //GenerateMap(minX, maxX, minY, maxY);
        }
        public void GenerateMap(Vector2Int chunkID)
        {
            Chunk chunk = GetChunk(chunkID);
            int minX = chunkID.x * width;
            int maxX = (chunkID.x + 1) * width - 1;
            int minY = chunkID.y * height;
            int maxY = (chunkID.y + 1) * height - 1;

            //if (chunk.fullTilemap == null) chunk.fullTilemap = Instantiate(fullTilemap,fullTilemap.transform.parent);
            for (int x = 0; x < width; x += 1)
            {
                for (int y = 0; y < height; y += 1)
                {
                    bool[] neighbors = chunk.GetTileNeighbors(x, y);
                    if (neighbors != null)
                    {
                        TileBase tile = tileRules.GetSprite(neighbors);
                        if (tile == null)
                        {
                            tile = fullTile;
                        }
                        /*chunk.*/
                        fullTilemap.SetTile(new Vector3Int(x + minX, -y - minY, 0), tile);
                    }
                    else
                    {
                        /*chunk.*/
                        fullTilemap.SetTile(new Vector3Int(x + minX, -y - minY, 0), null);
                    }

                }
            }

            chunk.BuildChunk(seed);
        }

        public void ClearMap(Vector2Int chunkID)
        {
            int minX = chunkID.x * width;
            int maxX = (chunkID.x + 1) * width - 1;
            int minY = chunkID.y * height;
            int maxY = (chunkID.y + 1) * height - 1;
            ClearMap(minX, maxX, minY, maxY);
            GetChunk(chunkID).ClearChunk();
        }

        public override void ClearMap()
        {
            int minX = chunkID.x * width;
            int maxX = (chunkID.x + 1) * width - 1;
            int minY = chunkID.y * height;
            int maxY = (chunkID.y + 1) * height - 1;
            ClearMap(minX, maxX, minY, maxY);
        }

        public bool[,] GenerateBoolMap(Vector2Int chunkID)
        {
            int minX = chunkID.x * width;
            int maxX = (chunkID.x + 1) * width - 1;
            int minY = chunkID.y * height;
            int maxY = (chunkID.y + 1) * height - 1;

            return GenerateBoolMap(minX, maxX, minY, maxY, 0);
        }

        Vector2Int lastClickChunkID;
        Vector2Int lastClickID;
        public enum HandleMouseClickFunction { placePlayer, resetChunk, placeLava, placeWater, toggleTile, displayPlatformGraph, printPlatformPath }
        public HandleMouseClickFunction clickFunction;
        private void HandleMouseClickDebugging()
        {
            if (clickFunction == HandleMouseClickFunction.resetChunk)
            {
                Vector2Int clickChunkID = GetChunkID(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (Input.GetMouseButton(0) && (lastClickChunkID == null || lastClickChunkID != clickChunkID))
                {
                    Chunk clickedChunk = GetChunk(clickChunkID);
                    if (clickedChunk != null)
                    {
                        Debug.Log($"resetting {clickChunkID} chunk");
                        bool[,] resetBoolMap = GenerateBoolMap(clickChunkID);
                        clickedChunk.SetTiles(resetBoolMap);
                        visibleChunkIDs.Remove(clickChunkID);
                        toFixChunkIDs.Add(clickChunkID);
                        toDisplayChunks.Add(clickChunkID);
                    }
                    lastClickChunkID = clickChunkID;
                }
            }
            else if (clickFunction == HandleMouseClickFunction.toggleTile)
            {
                Vector2Int clickTile = new Vector2Int((int)Mathf.Floor(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), (int)Mathf.Floor(Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
                if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1) && (lastClickID == null || lastClickID != clickTile))
                {
                    Debug.Log($"TileClicked {clickTile}");
                    Chunk clickedChunk = GetChunk(GetChunkID(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
                    if (clickedChunk != null)
                    {
                        bool value = GetTile(clickTile);
                        SetTile(clickTile, !value);

                    }
                    lastClickID = clickTile;
                }

            }
            else if (clickFunction == HandleMouseClickFunction.placeWater)
            {
                if (Input.GetMouseButtonUp(1))
                {
                    Vector2Int clickTile = new Vector2Int((int)Mathf.Floor(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), (int)Mathf.Floor(Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
                    if (!GetTile(clickTile))
                    {
                        StartCoroutine(PlaceLiquid(waterTile, waterTilemap, clickTile));
                    }
                }

            }
            else if (clickFunction == HandleMouseClickFunction.placeLava)
            {
                if (Input.GetMouseButtonUp(1))
                {
                    Vector2Int clickTile = new Vector2Int((int)Mathf.Floor(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), (int)Mathf.Floor(Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
                    if (!GetTile(clickTile))
                    {
                        StartCoroutine(PlaceLiquid(lavaTile, lavaTilemap, clickTile));
                    }
                }

            }
            else if (clickFunction == HandleMouseClickFunction.displayPlatformGraph)
            {
                if (roomChunk != null && Input.GetMouseButtonUp(0))
                {
                    displayPlatformGraph = false;
                    Vector2Int clickTile = new Vector2Int((int)Mathf.Floor(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), (int)Mathf.Floor(Camera.main.ScreenToWorldPoint(Input.mousePosition).y));

                    startingPlatformID = roomChunk.GetPlatformID(clickTile);
                    int filledChunkID = startingPlatformID / 512;
                    int platformChunkId = startingPlatformID % 512;
                    Debug.LogWarning($"PlatformIDClicked: {startingPlatformID}  filledChunkID:{filledChunkID} platformID{platformChunkId}");
                    Thread thread = new Thread(GenerateChunkGraphThread);
                    thread.Start();
                }
            }
            else if (clickFunction == HandleMouseClickFunction.printPlatformPath)
            {
                if (roomChunk != null && Input.GetMouseButtonUp(0))
                {
                    Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2Int clickTile = new Vector2Int((int)Mathf.Floor(clickPos.x), (int)Mathf.Floor(clickPos.y));

                    PrintPlatform(clickTile);
                    GenerateConnectedChunkGraph();
                }
            }


        }

        public LocomotionSolver ls;
        public bool generatingLocomotionGraph = false;
        public IEnumerator GenerateLocomotionGraph()
        {
            generatingLocomotionGraph = true;
            generateLocomotionGraphThreadCompleted = false;
            Thread thread = new Thread(GenerateLocomotionGraphThread);
            thread.Start();
            while (!generateLocomotionGraphThreadCompleted)
            {
                yield return null;
            }

            ls.Solve(nodeChunks);

            while (!ls.ready)
            {
                yield return null;
            }

            generatingLocomotionGraph = false;
        }

        List<NodeChunk> nodeChunks;
        bool generateLocomotionGraphThreadCompleted = false;
        private void GenerateLocomotionGraphThread()
        {
            List<int> platformIDs = roomChunk.GetPlatformIDs();
            nodeChunks = new List<NodeChunk>();
            foreach (int platformID in platformIDs)
            {
                NodeChunk nodeChunk = roomChunk.GetPlatform(platformID);

            }
            roomChunk.SetPlatformEdges(platformIDs, jumpHeight, checkConnection);
            generateLocomotionGraphThreadCompleted = true;
        }


        private void PrintPlatform(Vector2Int startTile)
        {
            int platformID = roomChunk.GetPlatformID(startTile);
            if (platformID == 0)
            {
                roomChunk.PrintPath(startTile, jumpHeight, platformID);
            }
            else if (platformID % 512 > 0)
            {
                roomChunk.PrintPath(new Vector2Int(startTile.x, startTile.y + 1), jumpHeight, platformID);
            }
        }
        private int startingPlatformID;
        public int generateChunkGraphDepth = 0;
        public bool checkConnection = false;
        private void GenerateChunkGraphThread()
        {
            List<int> platformNodes = new List<int>();
            platformNodes.Add(startingPlatformID);

            int graphListIndex = 0;
            int depth = 0;
            while (depth < generateChunkGraphDepth)
            {
                int platformNodesCount = platformNodes.Count;
                for (int i = graphListIndex; i < platformNodesCount; i += 1)
                {
                    foreach (int nodeDestionationId in roomChunk.GetPlatformEdges(platformNodes[i], jumpHeight, checkConnection))
                    {
                        if (!platformNodes.Contains(nodeDestionationId)) platformNodes.Add(nodeDestionationId);
                    }
                }
                graphListIndex = platformNodesCount;
                depth += 1;
            }

            List<PlatformChunk> graphList = new List<PlatformChunk>();
            foreach (int nodeID in platformNodes)
            {
                PlatformChunk platform = roomChunk.GetPlatform(nodeID);
                if (!graphList.Contains(platform))
                {
                    roomChunk.GetPlatformEdges(nodeID, jumpHeight, checkConnection);
                    graphList.Add(platform);
                }
            }
            List<PlatformChunkGraph> platformChunkGraphs = new List<PlatformChunkGraph>();
            foreach (PlatformChunk platform in graphList)
            {
                PlatformChunkGraph platformChunkGraph = new PlatformChunkGraph();
                platformChunkGraph.platform = platform;
                platformChunkGraph.graphColor = Color.red;
                platformChunkGraphs.Add(platformChunkGraph);
            }
            platformGraph = platformChunkGraphs;
            displayPlatformGraph = true;
        }

        public void GenerateConnectedChunkGraph()
        {
            displayPlatformGraph = false;
            Thread thread = new Thread(GenerateConnectedChunkGraphThread);
            thread.Start();
        }
        private void GenerateConnectedChunkGraphThread()
        {
            //find all platformIDs
            List<int> platformIDs = roomChunk.GetPlatformIDs();

            List<int> sourceIDs = new List<int>();
            for (int i = 0; i < platformIDs.Count; i += 1)
            {
                int id = platformIDs[i];
                bool isSource = true;
                for (int j = 0; j < platformIDs.Count; j += 1)
                {
                    if (roomChunk.GetPlatformEdges(platformIDs[j], jumpHeight, checkConnection).Contains(id))
                    {
                        isSource = false;
                        break;
                    }
                }
                if (isSource)
                {
                    //Debug.Log($"SourceID {id}");
                    sourceIDs.Add(id);
                    //roomChunk.GetPlatform(id).defaultSources.Add(roomChunk.GetPlatform(id));
                }
            }

            //finds all sources for each platform and adds to defaultSources in the NodeChunk
            Debug.Log($"sourceIDs.Count:{sourceIDs.Count}  platformIDs.Count:{platformIDs.Count}");
            List<int> sourceIDsCopy = new List<int>(sourceIDs);
            while (sourceIDsCopy.Count > 0)
            {
                int currentSource = sourceIDsCopy[0];
                sourceIDsCopy.RemoveAt(0);
                List<int> frontier = new List<int>();
                frontier.Add(currentSource);
                while (frontier.Count > 0)
                {
                    int current = frontier[0];
                    frontier.RemoveAt(0);
                    PlatformChunk platform = roomChunk.GetPlatform(current);
                    if (!platform.defaultSources.Contains(roomChunk.GetPlatform(currentSource)))
                    {
                        platform.defaultSources.Add(roomChunk.GetPlatform(currentSource));
                        foreach (int connection in platform.connectedPlatforms)
                        {
                            frontier.Add(connection);
                        }
                    }

                }
            }

            //find all platform connections
            List<PlatformChunkGraph> platforms = new List<PlatformChunkGraph>();
            int connectChunkID = 0;
            Color[] colors = { Color.red, Color.blue, Color.cyan, Color.green, Color.magenta, Color.yellow, Color.white, Color.black };
            
            List<PlatformChunkGraph> chunkGroups = new List<PlatformChunkGraph>();
            foreach (int platformID in platformIDs)
            {
                PlatformChunk platform = roomChunk.GetPlatform(platformID);
                PlatformChunkGraph platformChunkGraph = new PlatformChunkGraph();
                platformChunkGraph.platform = platform;
                platforms.Add(platformChunkGraph);

                bool foundChunkGroup = false;
                for (int i = 0; i < chunkGroups.Count; i++)
                {
                    if (platform != chunkGroups[i].platform && platform.defaultSources.Count == chunkGroups[i].platform.defaultSources.Count)
                    {
                        bool matching = true;
                        foreach (PlatformChunk source in platform.defaultSources)
                        {
                            if (!chunkGroups[i].platform.defaultSources.Contains(source))
                            {
                                matching = false;
                                break;
                            }
                        }
                        if (matching)
                        {
                            platformChunkGraph.graphColor = chunkGroups[i].graphColor;

                            foundChunkGroup = true;
                            break;
                        }
                    }

                }
                if (!foundChunkGroup)
                {
                    chunkGroups.Add(platformChunkGraph);
                    int colorID = (chunkGroups.Count - 1) % colors.Length;
                    platformChunkGraph.graphColor = colors[colorID];
                    //Debug.Log($"{platformChunkGraph.platform.nodeID}:{platformChunkGraph.graphColor}");
                }
            }
            Debug.Log($"chunkGroups.Count:{chunkGroups.Count}  platforms.Count:{platforms.Count}");

            platformGraph = platforms;
            displayPlatformGraph = true;
        }

        private IEnumerator PlaceLiquid(TileBase liquidTile, Tilemap tilemap, Vector2Int posStart)
        {
            yield return null;
            if (!tilemap.GetTile(new Vector3Int(posStart.x, posStart.y, 0)))
            {
                tilemap.SetTile(new Vector3Int(posStart.x, posStart.y, 0), liquidTile);
                if (!GetTile(posStart + Vector2Int.left)) StartCoroutine(PlaceLiquid(liquidTile, tilemap, posStart + Vector2Int.left));
                if (!GetTile(posStart + Vector2Int.right)) StartCoroutine(PlaceLiquid(liquidTile, tilemap, posStart + Vector2Int.right));
                if (!GetTile(posStart + Vector2Int.down)) StartCoroutine(PlaceLiquid(liquidTile, tilemap, posStart + Vector2Int.down));
            }



        }

        private void HandleFixTileRulesThread()
        {
            while (fixTileRules)
            {
                Vector2Int chunkID = Vector2Int.zero;
                bool chunkIDFound = false;
                lock (toFixChunkIDs)
                {
                    if (toFixChunkIDs.Count > 0)
                    {
                        chunkID = toFixChunkIDs[0];
                        chunkIDFound = true;
                    }
                }
                if (chunkIDFound)
                {
                    Chunk chunk = GetChunk(chunkID);
                    if (chunk != null)
                    {
                        lock (chunk)
                        {
                            Utility.CheckTileRules(chunk, tileRules); // need to check in case invalid were fixed in an overlapping subchunk
                            if (chunk.hasInvalidTile)
                            {
                                HandleFixTileRules(chunk);

                            }
                        }

                        lock (toFixChunkIDs)
                        {
                            toFixChunkIDs.RemoveAt(0);
                            chunk.hasInvalidTile = false;
                        }
                    }

                }

            }

        }

        public int fixTileRuleBorder = 2;
        private void HandleFixTileRules(Chunk chunk)
        {
            Debug.Log($"Fixing Chunk {chunk.chunkID}");
            List<SubChunk> subChunks = chunk.GetInvalidSubChunks(fixTileRuleBorder);
            Debug.Log($"chunkID = {chunk.chunkID} : subChunks.Count = {subChunks.Count}");
            foreach (SubChunk subChunk in subChunks)
            {
                if (Mathf.Abs(chunkID.x - chunk.chunkID.x) <= tileRadius.x && Mathf.Abs(chunkID.y - chunk.chunkID.y) <= tileRadius.y)
                {
                    subChunk.PrintTiles();
                }
            }
            if (!fixTileRules) return;
            foreach (SubChunk subChunk in subChunks)
            {
                fixSubChunk.Fix(subChunk, fixTileRuleBorder, tileRules);
                while (!fixSubChunk.ready)
                {/*waiting for fixSubChunk to be done*/ }
                if (!subChunk.hasInvalid)
                {
                    subChunk.PrintTiles();
                    for (int y = 0; y < subChunk.tiles.GetLength(1); y += 1)
                    {
                        for (int x = 0; x < subChunk.tiles.GetLength(0); x += 1)
                        {
                            //Debug.Log($"{x}x{y} = {subChunk.tiles[x, y]}");
                            chunk.SetTile(x + subChunk.minX, y + subChunk.minY, subChunk.tiles[x, y]);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("TileRule not fixed");
                }
            }
        }



    }
}
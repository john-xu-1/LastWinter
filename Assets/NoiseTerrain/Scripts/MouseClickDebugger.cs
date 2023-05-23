using NoiseTerrain;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MouseClickDebugger : MonoBehaviour
{
    public ChunkHandler chunks;
    public ProceduralMapGenerator generator;

    Vector2Int lastClickChunkID;
    Vector2Int lastClickID;
    public enum HandleMouseClickFunction { placePlayer, resetChunk, placeLava, placeWater, toggleTile, displayPlatformGraph, printPlatformPath, isValidWall }
    public HandleMouseClickFunction clickFunction;

    private bool displayPlatformGraph;
    List<PlatformChunkGraph> platformGraph;

    public class PlatformChunkGraph
    {
        public PlatformChunk platform;
        public Color graphColor;
    }

    private int startingPlatformID;
    public int generateChunkGraphDepth = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseClickDebugging();

        //debug platformGraph
        if (displayPlatformGraph)
        {
            foreach (PlatformChunkGraph platform in platformGraph)
            {
                foreach (int nodeID in platform.platform.connectedPlatforms)
                {
                    PlatformChunk destination = generator.RoomChunk.GetPlatform(nodeID);
                    Vector2 start = platform.platform.GetTilePos(platform.platform.jumpTiles[0]);
                    Vector2 dir = destination.GetTilePos(destination.jumpTiles[0]) - start;
                    Debug.DrawRay(start, dir, platform.graphColor);
                    //Debug.Log($"{platform.graphColor}");
                }
            }
        }
    }


    private void HandleMouseClickDebugging()
    {
        if (clickFunction == HandleMouseClickFunction.resetChunk)
        {
            Vector2Int clickChunkID = chunks.GetChunkID(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (Input.GetMouseButton(0) && (lastClickChunkID == null || lastClickChunkID != clickChunkID))
            {
                Chunk clickedChunk = chunks.GetChunk(clickChunkID);
                if (clickedChunk != null)
                {
                    Debug.Log($"resetting {clickChunkID} chunk");
                    bool[,] resetBoolMap = generator.GenerateBoolMap(clickChunkID);
                    clickedChunk.SetTiles(resetBoolMap);
                    generator.visibleChunkIDs.Remove(clickChunkID);
                    generator.toFixChunkIDs.Add(clickChunkID);
                    generator.toDisplayChunks.Add(clickChunkID);
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
                Chunk clickedChunk = chunks.GetChunk(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (clickedChunk != null)
                {
                    bool value = chunks.GetTile(clickTile);
                    chunks.SetTile(clickTile, !value);

                }
                lastClickID = clickTile;
            }

        }
        else if (clickFunction == HandleMouseClickFunction.placeWater)
        {
            if (Input.GetMouseButtonUp(1))
            {
                Vector2Int clickTile = new Vector2Int((int)Mathf.Floor(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), (int)Mathf.Floor(Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
                if (!chunks.GetTile(clickTile))
                {
                    generator.PlaceWater(clickTile);
                }
            }

        }
        else if (clickFunction == HandleMouseClickFunction.placeLava)
        {
            if (Input.GetMouseButtonUp(1))
            {
                Vector2Int clickTile = new Vector2Int((int)Mathf.Floor(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), (int)Mathf.Floor(Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
                if (!chunks.GetTile(clickTile))
                {
                    generator.PlaceLava(clickTile);
                }
            }

        }
        else if (clickFunction == HandleMouseClickFunction.displayPlatformGraph)
        {
            if (generator.RoomChunk != null && Input.GetMouseButtonUp(0))
            {
                displayPlatformGraph = false;
                Vector2Int clickTile = new Vector2Int((int)Mathf.Floor(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), (int)Mathf.Floor(Camera.main.ScreenToWorldPoint(Input.mousePosition).y));

                startingPlatformID = generator.RoomChunk.GetPlatformID(clickTile);
                int filledChunkID = startingPlatformID / 512;
                int platformChunkId = startingPlatformID % 512;
                Debug.LogWarning($"clickTile: {clickTile} PlatformIDClicked: {startingPlatformID}  filledChunkID:{filledChunkID} platformID{platformChunkId}");
                Thread thread = new Thread(GenerateChunkGraphThread);
                thread.Start();
            }
        }
        else if (clickFunction == HandleMouseClickFunction.printPlatformPath)
        {
            if (generator.RoomChunk != null && Input.GetMouseButtonUp(0))
            {
                Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2Int clickTile = new Vector2Int((int)Mathf.Floor(clickPos.x), (int)Mathf.Floor(clickPos.y));

                PrintPlatform(clickTile);
                GenerateConnectedChunkGraph();
            }
        }
        else if (clickFunction == HandleMouseClickFunction.isValidWall)
        {
            if (Input.GetMouseButtonUp(1)) //check right side of wall
            {
                Vector2Int clickTile = new Vector2Int((int)Mathf.Floor(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), (int)Mathf.Floor(Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
                if (chunks.GetTile(clickTile))
                {
                    Debug.Log($"isValidWall right empty: {WallChunk.IsValidWall(clickTile, generator.jumpHeight, true, generator.RoomChunk)}");
                }
            }else if (Input.GetMouseButtonUp(0)) //check left side of wall
            {
                Vector2Int clickTile = new Vector2Int((int)Mathf.Floor(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), (int)Mathf.Floor(Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
                if (chunks.GetTile(clickTile))
                {
                    Debug.Log($"isValidWall left empty: {WallChunk.IsValidWall(clickTile, generator.jumpHeight, false, generator.RoomChunk)}");
                }
            }

        }


    }

    
    

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
                foreach (int nodeDestionationId in generator.RoomChunk.GetPlatformEdges(platformNodes[i], generator.jumpHeight, generator.checkConnection))
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
            PlatformChunk platform = generator.RoomChunk.GetPlatform(nodeID);
            if (!graphList.Contains(platform))
            {
                generator.RoomChunk.GetPlatformEdges(nodeID, generator.jumpHeight, generator.checkConnection);
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
        List<int> platformIDs = generator.RoomChunk.GetPlatformIDs();

        List<int> sourceIDs = new List<int>();
        for (int i = 0; i < platformIDs.Count; i += 1)
        {
            int id = platformIDs[i];
            bool isSource = true;
            for (int j = 0; j < platformIDs.Count; j += 1)
            {
                if (generator.RoomChunk.GetPlatformEdges(platformIDs[j], generator.jumpHeight, generator.checkConnection).Contains(id))
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
                PlatformChunk platform = generator.RoomChunk.GetPlatform(current);
                if (!platform.defaultSources.Contains(generator.RoomChunk.GetPlatform(currentSource)))
                {
                    platform.defaultSources.Add(generator.RoomChunk.GetPlatform(currentSource));
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
            PlatformChunk platform = generator.RoomChunk.GetPlatform(platformID);
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

    private void PrintPlatform(Vector2Int startTile)
    {
        int platformID = generator.RoomChunk.GetPlatformID(startTile);
        if (platformID == 0)
        {
            generator.RoomChunk.PrintPath(startTile, generator.jumpHeight, platformID);
        }
        else if (platformID % 512 > 0)
        {
            generator.RoomChunk.PrintPath(new Vector2Int(startTile.x, startTile.y + 1), generator.jumpHeight, platformID);
        }
    }
}

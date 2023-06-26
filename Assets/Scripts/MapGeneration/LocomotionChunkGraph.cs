using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocomotionGraph;
using ChunkHandler;
using System.Threading;

public class LocomotionChunkGraph : LocomotionGraph.LocomotionGraph, IChunkable
{
    List<Chunk> roomChunks;

    public void GenerateFromChunks(List<Chunk> chunks)
    {
        int minYID = int.MaxValue;
        int minXID = int.MaxValue;
        int maxYID = int.MinValue;
        int maxXID = int.MinValue;

        Debug.Log($"roomChunks.Count: {roomChunks.Count}");
        foreach (Chunk chunk in roomChunks)
        {
            minXID = Mathf.Min(chunk.chunkID.x, minXID);
            minYID = Mathf.Min(chunk.chunkID.y, minYID);
            maxXID = Mathf.Max(chunk.chunkID.x, maxXID);
            maxYID = Mathf.Max(chunk.chunkID.y, maxYID);
        }

        Vector2Int roomChunkSize = new Vector2Int(maxXID - minXID + 1, maxYID - minYID + 1);
        //chunks = new Chunk[roomChunkSize.x, roomChunkSize.y];
        int width = roomChunkSize.x * roomChunks[0].width;
        int height = roomChunkSize.y * roomChunks[0].height;

        boolMapThread = new bool[width, height];

        Debug.Log($"minXID: {minXID} minYID: {minYID} maxXID: {maxXID} maxYID: {maxYID}");
        foreach (Chunk chunk in roomChunks)
        {
            //use chunk data to populate the boolMapThread
            
        }

        //calc the min/max tiles maxY and min Y are flipped since positive y is down
        minTileThread = new Vector2Int(minXID * roomChunks[0].width, maxYID * roomChunks[0].height + roomChunks[0].height - 1);
        maxTileThread = new Vector2Int(maxXID * roomChunks[0].width + roomChunks[0].width - 1, minYID * roomChunks[0].height);


    }

    public void SetRoomChunk(List<Chunk> roomChunks, int seed)
    {

        this.seed = seed;
        //platformSetupComplete = false;

        //active = true;
        this.roomChunks = roomChunks;
        Thread thread = new Thread(SetRoomChunkThread);
        thread.Start();
    }
    
    

    
}

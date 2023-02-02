using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoiseTerrain
{
    public class PlatformChunk : NodeChunk
    {
        public List<Vector2Int> groundTiles = new List<Vector2Int>();
        public PlatformChunk(RoomChunk roomChunk)
        {
            this.roomChunk = roomChunk;
        }
        
        public override void SetPath(int nodeID, int jumpHeight)
        {
            
            this.nodeID = nodeID;
            Vector2Int start = GetTilePos(groundTiles[0]);
            //roomChunk.PrintPath(new Vector2Int(groundTiles[0].x + roomChunk.minTile.x, -groundTiles[0].y + 1 - roomChunk.maxTile.y), jumpHeight, platformID);
            //path = roomChunk.GetPath(new Vector2Int(groundTiles[0].x + roomChunk.minTile.x, -groundTiles[0].y + 1 - roomChunk.maxTile.y), jumpHeight, platformID);
            
            //roomChunk.PrintPath(start, jumpHeight, platformID);
            path = roomChunk.GetPath(start, jumpHeight, nodeID);
            SetPlatformEdges(nodeID, roomChunk);
        }
        public void SetPlatformEdges(int platformID, RoomChunk roomChunk)
        {
            connectedPlatforms = new List<int>();
            for (int x = 0; x < path.GetLength(0); x += 1)
            {
                for (int y = 0; y < path.GetLength(1); y += 1)
                {
                    if (path[x, y] == 0 && y + 1 < roomChunk.height && roomChunk.GetTile(x, y + 1) && roomChunk.GetPlatformID(x, y + 1) != platformID)
                    {
                        int landingID = roomChunk.GetPlatformID(x, y + 1);
                        if (!connectedPlatforms.Contains(landingID)) connectedPlatforms.Add(landingID);
                    }
                }
            }
        }
    }
}
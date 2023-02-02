using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoiseTerrain
{
    public class NodeChunk
    {
        public int nodeID;
        public List<Vector2Int> jumpTiles = new List<Vector2Int>();
        public int[,] path;
        public List<int> connectedPlatforms;
        protected RoomChunk roomChunk;

        public virtual void SetPath(int platformID, int jumpHeight)
        {

        }

        public Vector2Int GetTilePos(Vector2Int tile)
        {
            return new Vector2Int(tile.x + roomChunk.minTile.x, -tile.y + 1 - roomChunk.maxTile.y);
        }
    }
}


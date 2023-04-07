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
                        if (!connectedPlatforms.Contains(landingID) && CheckConnection(x,y)) connectedPlatforms.Add(landingID);
                    }
                }
            }
        }
        bool debugCheckConnection = false;
        private bool CheckConnection(int x, int y)
        {
            if (debugCheckConnection || !checkConnection) return true;
            bool validConnection = false;
            int exitCounter = 0;
            int xStart = x;
            while (exitCounter < 10000)
            {

                if (Mathf.Abs(x - xStart) > 10) return false;
                //move up until platformID is found or reached a peak in the path
                //exit if another platformID or filledChunkID is found
                if (y > 0)
                {
                    y -= 1;
                    if (path[x, y] > 0) return true; //in platform jump area
                    if (path[x, y] < 0)
                    {
                        if (!roomChunk.GetTile(x, y))//air is above
                        {
                            if (xStart != x && path[x, y] >= 0) return false;
                            //check left and right
                            if (x > 0 && path[x - 1, y] >= 0) x -= 1; //move left
                            else if (x < path.GetLength(0) - 1 && path[x + 1, y] >= 0) x += 1; //move right
                            else return IsPeak(x, y + 1);
                        }
                        else //platform is above
                        {
                            int collidedPlatformID = roomChunk.GetPlatformID(x, y);
                            if (nodeID == collidedPlatformID)
                            {
                                return true;
                            }
                            else if (nodeID / 512 != collidedPlatformID / 512) break;
                        }
                    }
                }
                else
                {
                    break;
                }
                exitCounter++;
            }
            return validConnection;
        }

        private bool IsPeak(int x, int y)
        {
            //check left and right of x for the edge of path
            int xLeft = x, xRight = x;
            while (xLeft >= 0 && path[xLeft, y] >= 0)
            {
                if (path[xLeft, y - 1] >= 0 ) return false;
                xLeft -= 1;
            }
            while (xRight < path.GetLength(0) && path[xRight, y] >= 0)
            {
                if (path[xRight, y - 1] >= 0) return false;
                xRight++;
            }
            return true;
        }

        public override Vector2Int GetFluidEdge(int sinkID)
        {

            Vector2Int rEdge = groundTiles[0];
            Vector2Int lEdge = groundTiles[0];
            foreach (Vector2Int ground in groundTiles)
            {
                if (ground.x > rEdge.x) rEdge = ground;
                if (ground.x < lEdge.x) lEdge = ground;
            }

            if (rEdge.x < path.GetLength(0) - 1 && !roomChunk.GetTile(rEdge.x + 1, rEdge.y) && FindSink(sinkID, new Vector2Int(rEdge.x + 1, rEdge.y)))
            {
                return new Vector2Int(rEdge.x + 1, rEdge.y);
            }
            else if(lEdge.x > 0 && !roomChunk.GetTile(lEdge.x - 1, lEdge.y) && FindSink(sinkID, new Vector2Int(lEdge.x - 1, lEdge.y)))
            {
                return new Vector2Int(lEdge.x - 1, lEdge.y);
            }
            Debug.Log("Sink not found");

            return rEdge;

        }

        private bool FindSink(int sinkID, Vector2Int edgeStart)
        {
            
            while (!roomChunk.GetTile(edgeStart.x, edgeStart.y) && edgeStart.y < path.GetLength(1) - 1)
            {
                
                edgeStart = new Vector2Int(edgeStart.x, edgeStart.y + 1);
            }

            if (roomChunk.GetTile (edgeStart.x, edgeStart.y))
            {
                return roomChunk.GetPlatformID(new Vector2Int(edgeStart.x, -edgeStart.y)) == sinkID;
            }
            else
            {
                //fell out of map
                return false;
            }
        }

    }

}
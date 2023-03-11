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
                        if (!connectedPlatforms.Contains(landingID) && CheckConnection(x, y)) connectedPlatforms.Add(landingID);
                    }
                }
            }
        }
        bool debugCheckConnection = false;
        private bool CheckConnection(int x, int y)
        {
            if (debugCheckConnection) return true;
            int xStart = x, yStart = y;
            int exitCounter = 0;
            string pathStr = $"(xStart,yStart) ({xStart},{yStart})";
            while(exitCounter < 10000)
            {
                exitCounter++;
                //move up until platformID is found or platform jump area or reached a peak in the path (return true)
                //exit if another platformID or filledChunkID is found (return false)
                if (y > 0)
                {
                    y -= 1;
                    pathStr += $"\n ({x},{y})";
                    //Debug.Log($"({x},{y}) {roomChunk.GetTile(x, y)} {path[x, y]}");
                    //jump area found
                    if (path[x, y] > 0)
                    {
                        Debug.Log($"path found ({x},{y}) {pathStr}");
                        return true;
                    }
                    if(path[x,y] < 0) //either air or tile
                    {
                        if (!roomChunk.GetTile(x,y)) //air is above
                        {
                            if (xStart != x && path[x, y] >= 0) //check stepping
                            {
                                //todo check if left or right sides are filled
                                return false;
                            }
                                
                            //check left and right
                            if (x > 0 && path[x - 1, y] >= 0)//step right
                            {
                                x -= 1;
                                pathStr += $"step right";
                            }
                            else if(x < path.GetLength(0) - 1 && path[x + 1, y] >= 0)// step left;
                            {
                                x += 1;
                                pathStr += $"step left";
                            }
                                
                            else return IsPeak(x, y + 1,pathStr);
                        }
                        else //tile is above
                        {
                            int collidedPlatformID = roomChunk.GetPlatformID(x, y);
                            if (nodeID == collidedPlatformID)
                            {
                                Debug.Log($"nodeID found ({x},{y})  {pathStr}");
                                return true;
                            }
                            else if (nodeID / 512 != collidedPlatformID / 512) //wrong filledChunk
                            {
                                break;
                            }
                            else if (collidedPlatformID % 512 > 0 && nodeID != collidedPlatformID) //ground tile does not match target
                            {
                                break;
                            }
                                
                        }
                    }
                    
                }
                else break;
            }
            return false;
        }

        private bool IsPeak(int x, int y, string pathStr)
        {
            //check left and right of x for the edge of path
            int xLeft = x, xRight = x;
            while(xLeft >= 0 && path[xLeft,y] >= 0)
            {
                if (path[xLeft, y] > 0)// found jump area
                {
                    Debug.Log($"IsPeak left ({xLeft},{y}) x start: {x} {pathStr}");
                    return true; 
                }
                if (path[xLeft, y - 1] >= 0) return false; // found local max, not valid
                xLeft -= 1;
            }
            while (xRight < path.GetLength(0) && path[xRight, y] >= 0)
            {
                if (path[xRight, y] > 0)// found jump area
                {
                    Debug.Log($"IsPeak right ({xRight},{y}) x start: {x} {pathStr}");
                    return true; 
                }
                if (path[xRight, y - 1] >= 0) return false; // found local max, not valid
                xRight += 1;
            }
            if (xLeft < 0 || xRight >= path.GetLength(0)) return false;
            Debug.Log($"IsPeak left ({xLeft},{y})  right ({xRight},{y}) x start: {x} {pathStr}");
            return true;
        }
    }
}
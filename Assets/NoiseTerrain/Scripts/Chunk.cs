using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoiseTerrain
{
    public class Chunk
    {
        private ProceduralMapGenerator mapGenerator;

        public Vector2Int chunkID;
        private bool[,] boolMap;
        private bool[,] invalidTiles;

        public Chunk[] neighborChunks = new Chunk[8]; //0:upleft, 1:up, 2:upright, 3:left, 4:right, 5:downleft, 6:down, 7:downright

        int width, height;

        public Chunk(Vector2Int chunkID, bool[,]boolMap, ProceduralMapGenerator mapGenerator)
        {
            this.chunkID = chunkID;
            this.boolMap = boolMap;
            this.mapGenerator = mapGenerator;

            width = boolMap.GetLength(0);
            height = boolMap.GetLength(1);
        }

        public bool GetTile(int x, int y)
        {
            return boolMap[x, y];
        }

        public bool[] GetTileNeighbors(int x, int y)
        {
            if (boolMap[x,y])
            {
                
                bool[] neighbors = new bool[8];
                //upleft
                if (x > 0 && y > 0) neighbors[0] = boolMap[x - 1, y - 1];
                else if (x > 0) neighbors[0] = GetNeighborChunk(1).GetTile(x - 1, height - 1);
                else if (y > 0) neighbors[0] = GetNeighborChunk(3).GetTile(width - 1, y - 1);
                else neighbors[0] = GetNeighborChunk(0).GetTile(width - 1, height - 1);

                //up
                if (y > 0) neighbors[1] = boolMap[x, y - 1];
                else neighbors[1] = GetNeighborChunk(1).GetTile(x, height - 1);

                //upright
                if (y > 0 && x < width - 1) neighbors[2] = boolMap[x + 1, y - 1];
                else if (y > 0) neighbors[2] = GetNeighborChunk(4).GetTile(0, y - 1);
                else if (x < width - 1) neighbors[2] = GetNeighborChunk(1).GetTile(x + 1, height - 1);
                else neighbors[2] = GetNeighborChunk(2).GetTile(0, height - 1);

                //left
                if (x > 0) neighbors[3] = boolMap[x - 1, y];
                else neighbors[3] = GetNeighborChunk(3).GetTile(width - 1, y);

                //right
                if (x < width - 1) neighbors[4] = boolMap[x + 1, y];
                else neighbors[4] = GetNeighborChunk(4).GetTile(0, y);

                //downleft
                if (x > 0 && y < height - 1) neighbors[5] = boolMap[x - 1, y + 1];
                else if (x > 0) neighbors[5] = GetNeighborChunk(6).GetTile(x - 1, 0);
                else if (y < height - 1) neighbors[5] = GetNeighborChunk(3).GetTile(width - 1, y+1);
                else neighbors[5] = GetNeighborChunk(5).GetTile(width - 1, 0);

                //down
                if (y < height - 1) neighbors[6] = boolMap[x, y + 1];
                else neighbors[6] = GetNeighborChunk(6).GetTile(x, 0);

                //downright
                if (x < width - 1 && y < height - 1) neighbors[7] = boolMap[x + 1, y + 1];
                else if (x < width - 1) neighbors[7] = GetNeighborChunk(6).GetTile(x + 1, 0);
                else if (y < height - 1) neighbors[7] = GetNeighborChunk(4).GetTile(0, y + 1);
                else neighbors[7] = GetNeighborChunk(7).GetTile(0, 0);

                //string display = $"{neighbors[0]} {neighbors[1]} {neighbors[2]}\n{neighbors[3]} {boolMap[x, y]} {neighbors[4]}\n{neighbors[5]} {neighbors[6]} {neighbors[7]}";
                //Debug.Log(display);

                return neighbors;
            }
            else
            {
                return null;
            }
        }

        private Chunk GetNeighborChunk(int index)
        {
            if (neighborChunks[index] != null) return neighborChunks[index];
            else
            {
                switch (index)
                {
                    case 0:
                        neighborChunks[0] = mapGenerator.GetChunk(chunkID + new Vector2Int(-1, -1));
                        return neighborChunks[0];
                    case 1:
                        neighborChunks[1] = mapGenerator.GetChunk(chunkID + new Vector2Int(0, -1));
                        return neighborChunks[1];
                    case 2:
                        neighborChunks[2] = mapGenerator.GetChunk(chunkID + new Vector2Int(1, -1));
                        return neighborChunks[2];
                    case 3:
                        neighborChunks[3] = mapGenerator.GetChunk(chunkID + new Vector2Int(-1, 0));
                        return neighborChunks[3];
                    case 4:
                        neighborChunks[4] = mapGenerator.GetChunk(chunkID + new Vector2Int(1, 0));
                        return neighborChunks[4];
                    case 5:
                        neighborChunks[5] = mapGenerator.GetChunk(chunkID + new Vector2Int(-1, 1));
                        return neighborChunks[5];
                    case 6:
                        neighborChunks[6] = mapGenerator.GetChunk(chunkID + new Vector2Int(0, 1));
                        return neighborChunks[6];
                    case 7:
                        neighborChunks[7] = mapGenerator.GetChunk(chunkID + new Vector2Int(1, 1));
                        return neighborChunks[7];
                    default:
                        return null;
                }
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace NoiseTerrain
{
    public class ProceduralMapGenerator : MapGenerator
    {
        public Vector2Int chunkID;
        public Vector2Int chunkRadius = new Vector2Int(2, 1);

        public List<Vector2Int> visibleChunkIDs = new List<Vector2Int>();

        public Transform target;

        public LightingLevelSetup lighting;
        public EnemySetup enemies;
        public ItemSetup items;

        public List<Chunk> chunks = new List<Chunk>();

        private void Update()
        {
            Vector2Int chunkID = GetChunkID(target.position);
            DisplayMap(chunkID);
        }

        public ASP.ASPTileRules tileRules;
        protected override TileBase GetTile(float[,] heightMap, int x, int y)
        {
            bool[] neighbors = new bool[8];
            int xMax = heightMap.GetLength(0) - 1;
            int yMax = heightMap.GetLength(1) - 1;
            if (x > 0 && y > 0 && heightMap[x-1, y-1] > 0) neighbors[0] = true;
            if (y > 0 && heightMap[x, y-1] > 0) neighbors[1] = true;
            if (x < xMax && y > 0 && heightMap[x+1, y-1] > 0) neighbors[2] = true;
            if (x > 0 && heightMap[x-1, y] > 0) neighbors[3] = true;
            if (x < xMax && heightMap[x+1, y] > 0) neighbors[4] = true;
            if (x > 0 && y < yMax && heightMap[x-1, y+1] > 0) neighbors[5] = true;
            if (y < yMax && heightMap[x, y+1] > 0) neighbors[6] = true;
            if (x < xMax && y < yMax && heightMap[x+1, y+1] > 0) neighbors[7] = true;

            return tileRules.GetSprite(neighbors);
        }

        Chunk GetChunk(Vector2Int chunkID)
        {
            foreach(Chunk chunk in chunks)
            {
                if (chunk.chunkID == chunkID) return chunk;
            }
            return null;
        }
        public Vector2Int GetChunkID(Vector2 pos)
        {
            int xOffset = pos.x < 0 ? -1 : 0;
            int yOffset = pos.y > 0 ? 1 : 0;
            
            
            return new Vector2Int(xOffset + (int)pos.x / width, -yOffset -(int)pos.y / height);
        }

        public void DisplayMap(Vector2Int chunkID)
        {
            if(chunkID != this.chunkID)
            {
                this.chunkID = chunkID;
                List<Vector2Int> toDisplayChunks = new List<Vector2Int>();
                for(int x = -chunkRadius.x; x <= chunkRadius.x; x += 1)
                {
                    for (int y = -chunkRadius.y; y <= chunkRadius.y; y += 1)
                    {
                        toDisplayChunks.Add(chunkID + new Vector2Int(x,y));
                    }
                }
                //find chunks to be removed
                for (int i = visibleChunkIDs.Count - 1; i >= 0; i -= 1)
                {
                    if (!toDisplayChunks.Contains(visibleChunkIDs[i])){
                        ClearMap(visibleChunkIDs[i]);
                        visibleChunkIDs.RemoveAt(i);
                    }
                }
                //display chunks
                for(int i = 0; i < toDisplayChunks.Count; i+= 1)
                {
                    if (!visibleChunkIDs.Contains(toDisplayChunks[i]))
                    {
                        GenerateMap(toDisplayChunks[i]);
                        visibleChunkIDs.Add(toDisplayChunks[i]);
                    }
                }
            }

            
        }
        public override void GenerateMap()
        {
            int minX = chunkID.x * width;
            int maxX = (chunkID.x + 1) * width - 1;
            int minY = chunkID.y * height;
            int maxY = (chunkID.y + 1) * height - 1;
            GenerateMap(minX, maxX, minY, maxY);
        }
        public void GenerateMap(Vector2Int chunkID)
        {
            int minX = chunkID.x * width;
            int maxX = (chunkID.x + 1) * width - 1;
            int minY = chunkID.y * height;
            int maxY = (chunkID.y + 1) * height - 1;
            GenerateMap(minX, maxX, minY, maxY);
            Chunk chunk = GetChunk(chunkID);
            if (chunk == null)
            {
                LightingLevelSetup.LightingChunk<GameObject> lightingChunk = lighting.setupLighting(minX, maxX, minY + 1, maxY, seed);
                StartCoroutine(enemies.InitializeSetup(minX, maxX, minY + 1, maxY, seed));
                StartCoroutine(items.InitializeSetup(minX, maxX, minY + 1, maxY, seed));
                chunk = new Chunk();
                chunk.chunkID = chunkID;
                chunk.lightingChunk = lightingChunk;
                chunks.Add(chunk);
            }
            else
            {
                chunk.LoadChunk(lighting);
            }
            
        }

        public void ClearMap(Vector2Int chunkID)
        {
            int minX = chunkID.x * width;
            int maxX = (chunkID.x + 1) * width - 1;
            int minY = chunkID.y * height;
            int maxY = (chunkID.y + 1) * height - 1;
            ClearMap(minX, maxX, minY, maxY);
            Chunk chunk = GetChunk(chunkID);
            chunk.ClearChunk(lighting);
        }

        public override void ClearMap()
        {
            int minX = chunkID.x * width;
            int maxX = (chunkID.x + 1) * width - 1;
            int minY = chunkID.y * height;
            int maxY = (chunkID.y + 1) * height - 1;
            ClearMap(minX, maxX, minY, maxY);
        }
        
    }
}
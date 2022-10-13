using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                LightingLevelSetup.LightingChunk<GameObject> lightingChunk = lighting.setupLighting(minX, maxX, minY, maxY, seed);
                StartCoroutine(enemies.InitializeSetup(minX, maxX, minY, maxY, seed));
                StartCoroutine(items.InitializeSetup(minX, maxX, minY, maxY, seed));
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
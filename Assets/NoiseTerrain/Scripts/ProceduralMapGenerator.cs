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

        public TileRules tileRules;

        List<Chunks> chunks = new List<Chunks>();

        private void Update()
        {
            Vector2Int chunkID = GetChunkID(target.position);
            DisplayMap(chunkID);
        }

        public Chunks GetChunk (Vector2Int chunkID)
        {
            foreach (Chunks chunk in chunks)
            {
                if (chunk.chunkID == chunkID) return chunk;
            }
            return null;
        }

        public Vector2Int GetChunkID(Vector2 pos)
        {
            return new Vector2Int((int)pos.x / width, -(int)pos.y / height);
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
            lighting.setupLighting(minX, maxX, minY, maxY, seed);
        }

        public void ClearMap(Vector2Int chunkID)
        {
            int minX = chunkID.x * width;
            int maxX = (chunkID.x + 1) * width - 1;
            int minY = chunkID.y * height;
            int maxY = (chunkID.y + 1) * height - 1;
            ClearMap(minX, maxX, minY, maxY);
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
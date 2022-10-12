using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace NoiseTerrain
{
    public class ProceduralMapGenerator : MapGenerator
    {
        public Vector2Int chunkSize;
        public Vector2Int chunkID;
        public Transform target;

        public List<Vector2Int> displayChunks;
        public Vector2Int chunkRadius = new Vector2Int(1,1);
        //private Vector2Int currentChunkID;
        private void Update()
        {
            HandleDisplayChunks();
        }

        private void HandleDisplayChunks()
        {
            Vector2Int targetPos = new Vector2Int((int)target.position.x / chunkSize.x, -(int)target.position.y / chunkSize.y);
            if(chunkID.x != targetPos.x || chunkID.y != targetPos.y)
            {
                chunkID = targetPos;
                List<Vector2Int> toDisplayChunks = new List<Vector2Int>();
                if (targetPos.x > 0 && targetPos.y > 0 && targetPos.x < width / chunkSize.x && targetPos.y < height / chunkSize.y) toDisplayChunks.Add(targetPos);
                for (int x = -chunkRadius.x; x <= chunkRadius.x; x += 1)
                {
                    for (int y = -chunkRadius.y; y <= chunkRadius.y; y += 1)
                    {
                        if ((x != 0 || y != 0) && (targetPos.x + x >= 0 && targetPos.y + y >= 0 && targetPos.x + x < width / chunkSize.x && targetPos.y + y < height /chunkSize.y))
                        {
                            toDisplayChunks.Add(new Vector2Int(x, y) + targetPos);
                        }
                    }
                }

                for (int i = displayChunks.Count - 1; i >= 0; i--)
                {
                    if (!toDisplayChunks.Contains(displayChunks[i]))
                    {
                        ClearMap(displayChunks[i]);
                        displayChunks.RemoveAt(i);
                    }
                }
                for (int i = 0; i < toDisplayChunks.Count; i += 1)
                {
                    if (!displayChunks.Contains(toDisplayChunks[i]))
                    {
                        GenerateMap(toDisplayChunks[i]);
                        displayChunks.Add(toDisplayChunks[i]);
                    }
                }
            }
            
            //if (!displayChunks.Contains(targetPos))
            //{
            //    Debug.Log("placing chunk");
            //    GenerateMap(targetPos);
            //    displayChunks.Add(targetPos);
            //}
        }
        protected override void SetTile(Vector3Int pos, TileBase tileType)
        {
            
            pos = new Vector3Int(pos.x /*+ chunkID.x * chunkSize.x/2*/ /*- width / 2*/, pos.y /*+ chunkID.y * chunkSize.y/2*/ /*+ height / 2*/, pos.z);
            base.SetTile(pos, tileType);
        }
        protected override Vector2 GetOffset()
        {
            return new Vector2(chunkID.x *(width/noiseScale), chunkID.y * (-height/noiseScale));
        }
        public override void GenerateMap()
        {
            GenerateMap(chunkID);
        }

        public void GenerateMap(Vector2Int chunkID)
        {
            int minX = (chunkID.x) * chunkSize.x;
            int maxX = (chunkID.x+1) * chunkSize.x;
            int minY = (chunkID.y) * chunkSize.y;
            int maxY = (chunkID.y+1) * chunkSize.y;
            GenerateMap(minX, maxX, minY, maxY); 
        }

        public void ClearMap(Vector2Int chunkID)
        {
            int minX = (chunkID.x) * chunkSize.x;
            int maxX = (chunkID.x + 1) * chunkSize.x;
            int minY = (chunkID.y) * chunkSize.y;
            int maxY = (chunkID.y + 1) * chunkSize.y;
            ClearMap(minX, maxX, minY, maxY);
        }

        public virtual void ClearMap(int minX, int maxX, int minY, int maxY)
        {
            for (int y = minY; y < maxY; y += 1)
            {
                for (int x = minX; x < maxX; x += 1)
                {
                    SetTile(new Vector3Int(x, -y, 0), null);
                }
            }
        }

        float[,] noiseMap;
        public void GenerateMap(int minX, int maxX, int minY, int maxY)
        {
            //Debug.Log("GenerateMap");
            if(noiseMap == null) noiseMap = Sebastian.Noise.GenerateNoiseMap(width, height, seed, noiseScale, octaves, persistance, lacunarity, offset);

            for (int y = minY; y < maxY; y += 1)
            {
                for (int x = minX; x < maxX; x += 1)
                {
                    float noiseHeight = noiseMap[x, y];
                    if (noiseHeight > 0)
                    {
                        SetTile(new Vector3Int(x, -y, 0), fullTile);
                    }
                    else
                    {
                        SetTile(new Vector3Int(x, -y, 0), null);
                    }
                }
            }

            setupComplete = true;
        }
    }
}


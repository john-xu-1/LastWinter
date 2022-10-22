using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace NoiseTerrain
{
    public class ProceduralMapGenerator : MapGenerator
    {
        public Vector2Int chunkID;
        public Vector2Int chunkRadius = new Vector2Int(5, 4);
        public Vector2Int tileRulesRadius = new Vector2Int(4, 3);
        public Vector2Int tileRulesFixRadius = new Vector2Int(3, 2);
        public Vector2Int tileRadius = new Vector2Int(2, 1);

        public List<Vector2Int> visibleChunkIDs = new List<Vector2Int>();

        public Transform target;

        public LightingLevelSetup lighting;
        public TileRules tileRules;

        List<Chunk> chunks = new List<Chunk>();

        private void Update()
        {
            Vector2Int chunkID = GetChunkID(target.position);
            DisplayMap(chunkID);
        }

        public Chunk GetChunk(Vector2Int chunkID)
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

            return new Vector2Int(xOffset + (int)pos.x / width, -yOffset - (int)pos.y / height);
        }

        public void DisplayMap(Vector2Int chunkID)
        {
            if(chunkID != this.chunkID)
            {
                this.chunkID = chunkID;

                List<Vector2Int> toDisplayChunks = new List<Vector2Int>();
                for(int x = -tileRadius.x; x <= tileRadius.x; x += 1)
                {
                    for (int y = -tileRadius.y; y <= tileRadius.y; y += 1)
                    {
                        toDisplayChunks.Add(chunkID + new Vector2Int(x,y));
                    }
                }

                List<Vector2Int> toCheckTileRulesChunks = new List<Vector2Int>();
                for (int x = -tileRulesRadius.x; x <= tileRulesRadius.x; x += 1)
                {
                    for (int y = -tileRulesRadius.y; y <= tileRulesRadius.y; y += 1)
                    {
                        toCheckTileRulesChunks.Add(chunkID + new Vector2Int(x, y));
                    }
                }

                List<Vector2Int> toFixTileRulesChunks = new List<Vector2Int>();
                for (int x = -tileRulesFixRadius.x; x <= tileRulesFixRadius.x; x += 1)
                {
                    for (int y = -tileRulesFixRadius.y; y <= tileRulesFixRadius.y; y += 1)
                    {
                        toFixTileRulesChunks.Add(chunkID + new Vector2Int(x, y));
                    }
                }

                List<Vector2Int> toBuildChunks = new List<Vector2Int>();
                for (int x = -chunkRadius.x; x <= chunkRadius.x; x += 1)
                {
                    for (int y = -chunkRadius.y; y <= chunkRadius.y; y += 1)
                    {
                        toBuildChunks.Add(chunkID + new Vector2Int(x, y));
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
                //build chunks
                for(int i = 0; i < toBuildChunks.Count; i+= 1)
                {
                    Chunk chunk = GetChunk(toBuildChunks[i]);
                    if (chunk == null)
                    {
                        int minX = toBuildChunks[i].x * width;
                        int maxX = (toBuildChunks[i].x + 1) * width - 1;
                        int minY = toBuildChunks[i].y * height;
                        int maxY = (toBuildChunks[i].y + 1) * height - 1;
                        chunk = new Chunk(toBuildChunks[i], GenerateBoolMap(minX, maxX, minY, maxY), this);
                        chunks.Add(chunk);
                    }
                }

                //check tileRules
                for (int i = 0; i < toCheckTileRulesChunks.Count; i += 1)
                {
                    Chunk chunk = GetChunk(toCheckTileRulesChunks[i]);
                    CheckTileRules(chunk);
                   // Debug.Log($"Checking Chunk {chunk.chunkID}");
                }

                //fix tileRules
                for (int i = 0; i < toFixTileRulesChunks.Count; i += 1)
                {
                    Chunk chunk = GetChunk(toFixTileRulesChunks[i]);
                    if(chunk.hasInvalidTile)
                    {
                        //Debug.Log($"Fixing Chunk {chunk.chunkID}");
                        List<SubChunk> subChunks = chunk.GetInvalidSubChunks(1);
                        Debug.Log($"chunkID = {chunk.chunkID} : subChunks.Count = {subChunks.Count}");
                        foreach(SubChunk subChunk in subChunks)
                        {
                            if(Mathf.Abs(chunkID.x - chunk.chunkID.x) <= tileRadius.x && Mathf.Abs(chunkID.y - chunk.chunkID.y) <= tileRadius.y)
                            {
                                string map = "";
                                int width = subChunk.tiles.GetLength(0);
                                int height = subChunk.tiles.GetLength(1);
                                for(int y = 0; y < height; y += 1)
                                {
                                    for(int x = 0; x < width; x += 1)
                                    {
                                        map += subChunk.tiles[x, y] ? 1 : 0;
                                    }
                                    map += "\n";
                                }
                                Debug.Log(map);
                            }
                        }
                    }
                }

                //display tiles
                for (int i = 0; i < toDisplayChunks.Count; i += 1)
                {
                    if (!visibleChunkIDs.Contains(toDisplayChunks[i]))
                    {
                        GenerateMap(toDisplayChunks[i]);
                        visibleChunkIDs.Add(toDisplayChunks[i]);
                    }
                }
            }

            
        }

        void CheckTileRules(Chunk chunk)
        {

            for(int x = 0; x < width; x += 1)
            {
                for(int y = 0; y < height; y += 1)
                {
                    bool[] neighbors = chunk.GetTileNeighbors(x, y);
                    if(neighbors != null)
                    {
                        bool valid = tileRules.GetValidTile(neighbors);
                        chunk.SetInvalidTile(x, y, !valid);

                        if (!valid) chunk.hasInvalidTile = true;
                    }
                    
                }
            }
            //chunk.SetInvalidTile();
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
            Chunk chunk = GetChunk(chunkID);
            int minX = chunkID.x * width;
            int maxX = (chunkID.x + 1) * width - 1;
            int minY = chunkID.y * height;
            int maxY = (chunkID.y + 1) * height - 1;

            for(int x = 0; x < width; x += 1)
            {
                for(int y = 0; y < height; y += 1)
                {
                    bool[] neighbors = chunk.GetTileNeighbors(x, y);
                    if(neighbors != null)
                    {
                        TileBase tile = tileRules.GetSprite(neighbors);
                        if (tile == null)
                        {
                            tile = fullTile;
                        }
                        fullTilemap.SetTile(new Vector3Int(x + minX, -y - minY, 0), tile);
                    }
                    else
                    {
                        fullTilemap.SetTile(new Vector3Int(x + minX, -y - minY, 0), null);
                    }
                    
                }
            }

            //GenerateMap(minX, maxX, minY, maxY);
            lighting.setupLighting(minX, maxX, minY+1, maxY, seed);
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
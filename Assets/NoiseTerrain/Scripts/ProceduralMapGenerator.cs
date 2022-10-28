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
        public EnemySetup enemySetup;
        public TileRules tileRules;

        List<Chunk> chunks = new List<Chunk>();
        public bool debugFixTileRules;

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
                    CheckTileRules(chunk); // need to check in case invalid were fixed in an overlapping subchunk
                    if (chunk.hasInvalidTile)
                    {
                        HandleFixTileRules(chunk);

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
        bool CheckTileRules(SubChunk subChunk)
        {
            for (int x = 1; x < subChunk.tiles.GetLength(0)-1; x += 1)
            {
                for (int y = 1; y < subChunk.tiles.GetLength(1) - 1; y += 1)
                {
                    bool[] neighbors = subChunk.GetTileNeighbors(x, y);
                    if (neighbors != null && !tileRules.GetValidTile(neighbors)) return false;
                    
                }
            }
            return true;
        }

        bool CheckTileRules(List<bool> tiles, int width)
        {
            int height = tiles.Count / width;
            for (int x = 1; x < width-1; x += 1)
            {
                for (int y = 1; y < height-1; y += 1)
                {
                    int index = x + y * width;
                    bool[] neighbors = SubChunk.GetTileNeighbors(index, width, tiles);
                    if (neighbors != null && !tileRules.GetValidTile(neighbors)) return false;

                }
            }
            return true;
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
            if(!chunk.initialized)StartCoroutine(enemySetup.InitializeSetup(minX,maxX, minY, maxY, seed));
            chunk.BuildChunk(lighting, enemySetup, seed);
        }

        public void ClearMap(Vector2Int chunkID)
        {
            int minX = chunkID.x * width;
            int maxX = (chunkID.x + 1) * width - 1;
            int minY = chunkID.y * height;
            int maxY = (chunkID.y + 1) * height - 1;
            ClearMap(minX, maxX, minY, maxY);
            GetChunk(chunkID).ClearChunk(lighting);
        }

        public override void ClearMap()
        {
            int minX = chunkID.x * width;
            int maxX = (chunkID.x + 1) * width - 1;
            int minY = chunkID.y * height;
            int maxY = (chunkID.y + 1) * height - 1;
            ClearMap(minX, maxX, minY, maxY);
        }

        public int fixTileRuleBorder = 2;
        private void HandleFixTileRules(Chunk chunk)
        {
            //Debug.Log($"Fixing Chunk {chunk.chunkID}");
            List<SubChunk> subChunks = chunk.GetInvalidSubChunks(fixTileRuleBorder);
            Debug.Log($"chunkID = {chunk.chunkID} : subChunks.Count = {subChunks.Count}");
            foreach (SubChunk subChunk in subChunks)
            {
                if (Mathf.Abs(chunkID.x - chunk.chunkID.x) <= tileRadius.x && Mathf.Abs(chunkID.y - chunk.chunkID.y) <= tileRadius.y)
                {
                    subChunk.PrintTiles();
                }
            }
            if (!debugFixTileRules) return;
            foreach (SubChunk subChunk in subChunks)
            {
                List<bool> tilesList = subChunk.GetTilesList();
                int width = subChunk.tiles.GetLength(0);
                int height = tilesList.Count / width;
                int count = tilesList.Count - height * 2 - width * 2 + 4;
                int[] indices = new int[count];
                int index = 0;
                int border = fixTileRuleBorder - 1;
                for (int y = border; y < height - border; y += 1)
                {
                    for (int x = border; x < width - border; x += 1)
                    {
                        indices[index] = width * y + x;
                        index += 1;
                    }
                }
                subChunk.hasInvalid = !CheckTileRules(subChunk);
                FixSubChunk(0, indices, tilesList, width, subChunk);
                if (!subChunk.hasInvalid)
                {
                    subChunk.PrintTiles();
                    for (int y = 0; y < height; y += 1)
                    {
                        for (int x = 0; x < width; x += 1)
                        {
                            chunk.SetTile(x + subChunk.minX, y + subChunk.minY, subChunk.tiles[x, y]);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("TileRule not fixed");
                }
            }
        }

        private void FixSubChunk(int index, int[] indices, List<bool> tiles, int width, SubChunk subChunk)
        {
            if (subChunk.hasInvalid)
            {
                subChunk.hasInvalid = !CheckTileRules(tiles, width);

                if (!subChunk.hasInvalid)
                {
                    Debug.Log("Chunk fixed");
                    for (int x = 0; x < width; x += 1)
                    {
                        for (int y = 0; y < tiles.Count / width; y += 1)
                        {
                            subChunk.tiles[x, y] = tiles[x + width * y];
                        }
                    }
                }
                else
                {
                    if (index < indices.Length)
                    {
                        if (subChunk.hasInvalid)
                        {
                            List<bool> tilesClone = new List<bool>(tiles);
                            tilesClone[indices[index]] = !tilesClone[indices[index]];
                            FixSubChunk(index += 1, indices, tilesClone, width, subChunk);
                        }

                        if (subChunk.hasInvalid && index < indices.Length)
                        {
                            List<bool> tilesClone = new List<bool>(tiles);
                            //tilesClone[indices[index + 1]] = !tilesClone[indices[index]];
                            FixSubChunk(index += 1, indices, tilesClone, width, subChunk);
                        }
                    }
                    
                }
            }
            
            

           
            
        }

    }
}
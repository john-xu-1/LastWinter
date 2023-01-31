using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace NoiseTerrain
{
    public class MapGenerator : MonoBehaviour
    {
        public TileBase fullTile;
        public Tilemap fullTilemap;

        public int width = 100, height = 100;
        public int minX { get { return GetMinX(); } }
        public int maxX { get { return GetMaxX(); } }
        public int minY { get { return GetMinY(); } }
        public int maxY { get { return GetMaxY(); } }
        protected virtual int GetMinX() { return 0; }
        protected virtual int GetMaxX() { return width - 1; }
        protected virtual int GetMinY() { return 0; }
        protected virtual int GetMaxY() { return height - 1; }

        public float noiseScale = 1;
        public int octaves = 1;

        [Range(0, 1)]
        public float persistance;
        public float lacunarity;
        public int seed;
        public Vector2 offset;

        public bool autoUpdate = false;

        public virtual void ClearMap()
        {
            for (int y = 0; y < height; y += 1)
            {
                for (int x = 0; x < width; x += 1)
                {
                    fullTilemap.SetTile(new Vector3Int(x, -y, 0), null);
                }
            }
        }
        public void ClearMap(int minX, int maxX, int minY, int maxY)
        {
            for (int y = minY; y <= maxY; y += 1)
            {
                for (int x = minX; x <= maxX; x += 1)
                {
                    fullTilemap.SetTile(new Vector3Int(x, -y, 0), null);
                }
            }
        }
        
        public bool[,] GenerateBoolMap(int minX, int maxX, int minY, int maxY, float threshold)
        {
            //Debug.Log("GenerateMap");
            int chunkSizeX = width;
            int chunkSizeY = height;

            Vector2 scaleOffset = new Vector2(minX / noiseScale, minY / noiseScale);

            float[,] noiseMap = Sebastian.Noise.GenerateNoiseMap(width, height, seed, noiseScale, octaves, persistance, lacunarity, scaleOffset + offset);
            bool[,] boolMap = new bool[width, height];
            for (int y = 0; y < height; y += 1)
            {
                for (int x = 0; x < width; x += 1)
                {
                    
                    boolMap[x, y] = noiseMap[x, y] > threshold;
                }
            }

            return boolMap;
        }

        public void GenerateMap(int minX, int maxX, int minY, int maxY)
        {
            //Debug.Log("GenerateMap");
            int chunkSizeX = width;
            int chunkSizeY = height;

            Vector2 scaleOffset = new Vector2(minX / noiseScale, minY / noiseScale);
            //Vector2Int chunkID = new Vector2Int(minX / width, minY / height);

            

            float[,] noiseMap = Sebastian.Noise.GenerateNoiseMap(width, height, seed, noiseScale, octaves, persistance, lacunarity, scaleOffset + offset);

            for (int y = 0; y < height; y += 1)
            {
                for (int x = 0; x < width; x += 1)
                {
                    float noiseHeight = noiseMap[x, y];
                    if (noiseHeight > 0)
                    {
                        fullTilemap.SetTile(new Vector3Int(x + minX, -y - minY, 0), fullTile);
                    }
                    else
                    {
                        fullTilemap.SetTile(new Vector3Int(x + minX, -y - minY, 0), null);
                    }
                }
            }

            setupComplete = true;
        }

        public virtual void GenerateMap()
        {
            //Debug.Log("GenerateMap");
            float[,] noiseMap = Sebastian.Noise.GenerateNoiseMap(width, height, seed, noiseScale, octaves, persistance, lacunarity, offset);

            for(int y = 0; y < height; y += 1)
            {
                for(int x = 0; x < width; x += 1)
                {
                    float noiseHeight = noiseMap[x, y];
                    if(noiseHeight > 0)
                    {
                        fullTilemap.SetTile(new Vector3Int(x, -y, 0), fullTile);
                    }
                    else
                    {
                        fullTilemap.SetTile(new Vector3Int(x, -y, 0), null);
                    }
                }
            }

            setupComplete = true;
        }

        public bool setupComplete = false;
        //public IEnumerator GenerateMap(int seed)
        //{
        //    setupComplete = false;
        //    this.seed = seed;
        //    GenerateMap();
        //    while (!setupComplete)
        //    {
        //        yield return null;
        //    }
        //}

        public virtual void GenerateMap(int seed)
        {
            setupComplete = false;
            this.seed = seed;
            GenerateMap();
        }

        public virtual void GenerateMap(Sebastian.MapDisplay display, int mapWidth, int mapHeight, int seed, float noiseScale, int octaves, float persistance, float lacunarity, Vector2 offset, float threshold)
        {
            float[,] noiseMap = Sebastian.Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
            float[,] invertYMap = new float[noiseMap.GetLength(0), noiseMap.GetLength(1)];
            for (int y = 0; y < mapHeight; y += 1)
            {
                for (int x = 0; x < mapWidth; x += 1)
                {
                    invertYMap[x, mapHeight - y - 1] = noiseMap[x, y] > threshold? 1 : -1;
                    noiseMap[x, y] = noiseMap[x, y] > threshold ? 1 : -1;
                }
            }
            display.DrawRawImage(Sebastian.TextureGenerator.TextureFromHeightMap(invertYMap));
        }
    }
}


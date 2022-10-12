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

        public float noiseScale = 1;
        public int octaves = 1;

        [Range(0, 1)]
        public float persistance;
        public float lacunarity;
        public int seed;
        public Vector2 offset;
        private Vector2 _offset { get { return GetOffset(); } }
        protected virtual Vector2 GetOffset() { return offset; }

        public bool autoUpdate = false;

        public virtual void ClearMap()
        {
            for (int y = 0; y < height; y += 1)
            {
                for (int x = 0; x < width; x += 1)
                {
                    SetTile(new Vector3Int(x, -y, 0), null);
                }
            }
        }
        public virtual void GenerateMap()
        {
            //Debug.Log("GenerateMap");
            float[,] noiseMap = Sebastian.Noise.GenerateNoiseMap(width, height, seed, noiseScale, octaves, persistance, lacunarity, _offset);

            for(int y = 0; y < height; y += 1)
            {
                for(int x = 0; x < width; x += 1)
                {
                    float noiseHeight = noiseMap[x, y];
                    if(noiseHeight > 0)
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

        protected virtual void SetTile(Vector3Int pos, TileBase tileType)
        {
            fullTilemap.SetTile(pos, tileType);
        }

        public bool setupComplete = false;
        public IEnumerator GenerateMap(int seed)
        {
            setupComplete = false;
            this.seed = seed;
            GenerateMap();
            while (!setupComplete)
            {
                yield return null;
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LightingLevelSetup : MonoBehaviour
{
    public Tilemap collisionMap, lavaMap, waterMap, doorMap;
    public int minX = 1, maxX = 160, minY = 1, maxY = 160;

    public GameObject[] lightPlants;
    private List<List<Lighting>> lightPlantPool = new List<List<Lighting>>();

    [SerializeField] bool isTestLight;

    public bool setupComplete = false;
    public NoiseTerrain.ProceduralMapGenerator map;

    List<ChunkObjectLights> chunkObjectLights = new List<ChunkObjectLights>();
    List<Vector2Int> chunkObjectLightsIDs = new List<Vector2Int>();
    public ChunkObjectLights chunkObjectLightsPrefab;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < lightPlants.Length; i += 1)
        {
            lightPlantPool.Add(new List<Lighting>());
        }
        if (isTestLight) setupLighting();
    }

    public void setupLighting(int maxX, int maxY)
    {
        this.maxX = maxX;
        this.maxY = maxY;
        StartCoroutine(setupLighting());
    }
    public struct Lighting
    {
        public GameObject light;
        public int lightID;
    }
    //public List<Lighting> setupLighting(List<NoiseTerrain.PlatformChunk> platforms, int seed)
    //{
    //    System.Random random = new System.Random(seed);
    //    List<Lighting> lightings = new List<Lighting>();
    //    foreach(NoiseTerrain.PlatformChunk platform in platforms)
    //    {
    //        int lights = 1;
    //        while(lights > 0)
    //        {
    //            foreach (Vector2Int ground in platform.groundTiles)
    //            {
    //                if (random.Next(0, 10) == 9)
    //                {
    //                    int index = random.Next(0, lightPlants.Length);
    //                    Lighting lighting = GetLight(index);
    //                    Vector2Int groundPos = platform.GetTilePos(ground);
    //                    lighting.light.transform.position = new Vector3(groundPos.x, groundPos.y, 0);
    //                    lightings.Add(lighting);
    //                    lights -= 1;
    //                }
    //            }
    //        }
            
    //    }
        
    //    setupComplete = true;
    //    return lightings;
    //}
    public ChunkObjectLights GetChunkObjectLights(Vector2Int chunkID)
    {
        ChunkObjectLights chunkObjectLights = null;
        for(int i = 0; i < chunkObjectLightsIDs.Count; i+=1)
        {
            Vector2Int chunkObjectID = chunkObjectLightsIDs[i];
            if (chunkObjectID == chunkID)
            {
                chunkObjectLights = this.chunkObjectLights[i];
                break;
            }
        }
        if(chunkObjectLights == null)
        {
            chunkObjectLights = Instantiate(chunkObjectLightsPrefab);
            chunkObjectLightsIDs.Add(chunkID);
            this.chunkObjectLights.Add(chunkObjectLights);
            NoiseTerrain.Chunk myChunk = NoiseTerrain.ChunkHandler.singlton.GetChunk(chunkID);
            myChunk.AddChunkObject(chunkObjectLights/*, 3*/);
            chunkObjectLights.mychunk = myChunk;
        }
        return chunkObjectLights;
    }
    public void SetLight(int type, Vector2 pos)
    {
        Vector2Int chunkID = NoiseTerrain.ChunkHandler.singlton.GetChunkID(pos);
        GetChunkObjectLights(chunkID).AddLight(type, pos);

    }
    public void setupLighting(List<NoiseTerrain.PlatformChunk> platforms, int seed)
    {
        System.Random random = new System.Random(seed);
        List<Lighting> lightings = new List<Lighting>();
        foreach (NoiseTerrain.PlatformChunk platform in platforms)
        {
            int lights = 1;
            while (lights > 0)
            {
                foreach (Vector2Int ground in platform.groundTiles)
                {
                    if (random.Next(0, 10) == 9)
                    {
                        int index = random.Next(0, lightPlants.Length);
                        //Lighting lighting = GetLight(index);
                        Vector2Int groundPos = platform.GetTilePos(ground);
                        SetLight(index, groundPos);
                        //lighting.light.transform.position = new Vector3(groundPos.x, groundPos.y, 0);
                        //lightings.Add(lighting);
                        lights -= 1;
                    }
                }
            }

        }

        setupComplete = true;
    }
    public List<Lighting> setupLighting(int minX, int maxX, int minY, int maxY, int seed)
    {
        System.Random random = new System.Random(seed);
        List<Lighting> lightings = new List<Lighting>();
        for (int i = minX; i < maxX; i += 1)
        {
            for (int j = minY; j < maxY; j += 1)
            {
                if (isGround(i, -j))
                {
                    if (random.Next(0,10) == 9)
                    {
                        int index = random.Next(0, lightPlants.Length);
                        Lighting lighting = GetLight(index);
                        lighting.light.transform.position = new Vector3(i, -j + 1, 0);
                        lightings.Add(lighting);
                    }
                }
            }
        }
        setupComplete = true;
        return lightings;
    }

    public IEnumerator setupLighting()
    {
        for (int i = minX; i < maxX; i += 1)
        {
            for (int j = minY; j < maxY; j += 1)
            {
                if (isGround(i, -j))
                {
                    if (placeLight(i, -j)) yield return null;
                }
            }
        }

        setupComplete = true;
    }

    bool isGround(int x, int y)
    {
        if (map == null)
        {
            TileBase groundTile = UtilityTilemap.GetTile(collisionMap, new Vector2(x, y));
            TileBase airTile = UtilityTilemap.GetTile(collisionMap, new Vector2(x, y + 1));
            return groundTile != null && airTile == null;
        }

        bool ground = NoiseTerrain.ChunkHandler.singlton.GetTile(new Vector2Int(x, y));
        bool air = !NoiseTerrain.ChunkHandler.singlton.GetTile(new Vector2Int(x, y+1));
        return ground && air;
    }

    bool placeLight (int x, int y)
    {
        int random = Random.Range(0, 10);
        if (random == 9)
        {
            random = Random.Range(0, lightPlants.Length);
            Instantiate(lightPlants[random], new Vector3(x, y+1, 0), Quaternion.identity);
            return true;
        }
        else
        {
            return false;
        }
    }

    private Lighting GetLight(int lightID)
    {
        
        if(lightPlantPool[lightID].Count > 0)
        {
            Lighting lighting = lightPlantPool[lightID][0];
            lightPlantPool[lightID].RemoveAt(0);
            lighting.light.SetActive(true);
            Debug.Log($"Light {lighting.lightID} received");
            return lighting;
        }
        else
        {
            Lighting lighting = new Lighting();
            GameObject light = Instantiate(lightPlants[lightID]);
            lighting.lightID = lightID;
            lighting.light = light;
            Debug.Log($"Light {lighting.lightID} instantiated");
            return lighting;
        }
    }

    public void ReturnLight(Lighting lighting)
    {
        Debug.Log($"Light {lighting.lightID} returned");
        lighting.light.SetActive(false);
        lightPlantPool[lighting.lightID].Add(lighting);
    }

}

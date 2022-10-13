using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LightingLevelSetup : MonoBehaviour
{
    public Tilemap collisionMap, lavaMap, waterMap, doorMap;
    public int minX = 1, maxX = 160, minY = 1, maxY = 160;

    public GameObject[] lightPlants;
    private List<List<GameObject>> lightPools;

    [SerializeField] bool isTestLight;

    public bool setupComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        lightPools = new List<List<GameObject>>();
        for(int i = 0; i < lightPlants.Length; i += 1)
        {
            lightPools.Add(new List<GameObject>());
        }
        if (isTestLight) setupLighting();
    }

    public void setupLighting(int maxX, int maxY)
    {
        this.maxX = maxX;
        this.maxY = maxY;
        StartCoroutine(setupLighting());
    }
    
    public LightingChunk<GameObject> setupLighting(int minX, int maxX, int minY, int maxY, int seed)
    {
        System.Random random = new System.Random(seed);
        LightingChunk<GameObject> lightingChunk = new LightingChunk<GameObject>();
        for (int i = minX; i < maxX; i += 1)
        {
            for (int j = minY; j < maxY; j += 1)
            {
                if (isGround(i, -j))
                {
                    if (random.Next(0,10) == 9)
                    {
                        int lightID = random.Next(0, lightPlants.Length);

                        GameObject light = SetLight(lightID, new Vector3(i, -j + 1, 0));
                        lightingChunk.lightingIDs.Add(lightID);
                        lightingChunk.lights.Add(light);
                        lightingChunk.lightPositions.Add(new Vector3(i, -j + 1, 0));
                    }
                }
            }
        }
        return lightingChunk;
    }
    public LightingChunk<GameObject> setupLighting(LightingChunk<GameObject> lightingChunk)
    {
        for (int i = 0; i < lightingChunk.lightingIDs.Count; i += 1)
        {
            int id = lightingChunk.lightingIDs[i];
            Vector3 pos = lightingChunk.lightPositions[i];
            lightingChunk.lights[i] = SetLight(id, pos);
        }
        return lightingChunk;
    }

    private GameObject SetLight(int lightID, Vector3 pos)
    {
        List<GameObject> lightPool = lightPools[lightID];
        if(lightPool.Count > 0)
        {
            GameObject light = lightPool[0];
            lightPool.RemoveAt(0);
            light.transform.position = pos;
            light.SetActive(true);
            return light;
        }
        else
        {
            return Instantiate(lightPlants[lightID], pos, Quaternion.identity);
        }
    }

    public void ReturnLights(LightingChunk<GameObject> lightingChunk)
    {
        for(int i = 0; i < lightingChunk.lightingIDs.Count; i += 1)
        {
            int id = lightingChunk.lightingIDs[i];
            GameObject light = lightingChunk.lights[i];
            lightPools[id].Add(light);
            light.SetActive(false);
        }
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
        TileBase ground = UtilityTilemap.GetTile(collisionMap, new Vector2(x, y));
        TileBase air = UtilityTilemap.GetTile(collisionMap, new Vector2(x, y + 1));
        return ground != null && air == null;
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

    public class LightingChunk<T>
    {
        public List<int> lightingIDs = new List<int>();
        public List<Vector3> lightPositions = new List<Vector3>();
        public List<T> lights = new List<T>();
    }
}

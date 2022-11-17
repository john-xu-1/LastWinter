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

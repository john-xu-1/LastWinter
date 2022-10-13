using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LightingLevelSetup : MonoBehaviour
{
    public Tilemap collisionMap, lavaMap, waterMap, doorMap;
    public int minX = 1, maxX = 160, minY = 1, maxY = 160;

    public GameObject[] lightPlants;

    [SerializeField] bool isTestLight;

    public bool setupComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        if (isTestLight) setupLighting();
    }

    public void setupLighting(int maxX, int maxY)
    {
        this.maxX = maxX;
        this.maxY = maxY;
        StartCoroutine(setupLighting());
    }
    public void setupLighting(int minX, int maxX, int minY, int maxY, int seed)
    {
        System.Random random = new System.Random(seed);

        for (int i = minX; i < maxX; i += 1)
        {
            for (int j = minY; j < maxY; j += 1)
            {
                if (isGround(i, -j))
                {
                    if (random.Next(0,10) == 9)
                    {
                        int index = random.Next(0, lightPlants.Length);
                        Instantiate(lightPlants[index], new Vector3(i, -j + 1, 0), Quaternion.identity);
                        
                    }
                }
            }
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
}

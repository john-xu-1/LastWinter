using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSetup : Setup
{
    public UnityEngine.Tilemaps.Tilemap collsionMap;
    public GameObject[] items;
    public int itemCount = 5;

    public List<int> placedItems = new List<int>();

    bool setupComplete = false;
    bool finalizedComplete = false;
    public bool SetupComplete() { return setupComplete; }
    public bool FinalizedComplete() { return finalizedComplete; }

    public IEnumerator InitializeSetup(Clingo_02.AnswerSet answerSet, int seed)
    {
        yield return null;
        System.Random random = new System.Random(seed);
        foreach (List<string> atom in answerSet.Value["piece"])
        {
            int platformID = 0;
            GameObject prefab = null;
            if (atom[0] == "drone_controller")
            {
                prefab = items[0];
                platformID = int.Parse(atom[1]);
            }
            else if (atom[0] == "magnetized_shifter")
            {
                prefab = items[1];
                platformID = int.Parse(atom[1]);
            }
            else if (atom[0] == "old_shotgun")
            {
                prefab = items[2];
                platformID = int.Parse(atom[1]);
            }
            else if (atom[0] == "rainy_day")
            {
                prefab = items[3];
                platformID = int.Parse(atom[1]);
            }
            else if (atom[0] == "vol")
            {
                prefab = items[4];
                platformID = int.Parse(atom[1]);
            }
            else if (atom[0] == "gaeas_touch")
            {
                prefab = items[5];
                platformID = int.Parse(atom[1]);
            }
            if (platformID > 0)
            {
                //NoiseTerrain.PlatformChunk platform = GameObject.FindObjectOfType<NoiseTerrain.ProceduralMapGenerator>().GetPlatform(platformID);
                //Vector2Int ground = platform.groundTiles[random.Next(0, platform.groundTiles.Count)];
                Vector2 groundPos = SceneLoader.GetRandomGround(platformID, random);  //platform.GetTilePos(ground);
                GameObject item = GameObject.Instantiate(prefab);
                item.transform.position = new Vector2(groundPos.x + 0.5f, groundPos.y + 2);
            }

        }
        setupComplete = true;
    }

    public IEnumerator InitializeSetup(List<LocomotionGraph.PlatformChunk> platforms, int seed)
    {
        yield return null;
        System.Random random = new System.Random(seed);
        List<int> usedPlatforms = new List<int>();
        while (itemCount > 0 && placedItems.Count < items.Length)
        {

            int rand = random.Next(0, items.Length);
            if (!placedItems.Contains(rand))
            {
                itemCount -= 1;
                placedItems.Add(rand);
                GameObject item = GameObject.Instantiate(items[rand]);
                LocomotionGraph.PlatformChunk platform = platforms[random.Next(0, platforms.Count)];
                Vector2Int ground = platform.jumpTiles[random.Next(0, platform.jumpTiles.Count)];
                Vector2Int groundPos = platform.GetTilePos(ground);
                item.transform.position = new Vector2(groundPos.x + 0.5f, groundPos.y + 2);
            }
        }
        setupComplete = true;
    }

    public IEnumerator InitializeSetup(int minX, int maxX, int minY, int maxY, int seed)
    {
        yield return null;

        while (itemCount > 0 && placedItems.Count < items.Length)
        {
            int x = Random.Range(minX, maxX);
            int y = Random.Range(minY, maxY);
            while (!UtilityTilemap.isGround(x, -y, collsionMap))
            {
                x = Random.Range(minX, maxX);
                y = Random.Range(minY, maxY);
            }
            int rand = Random.Range(0, items.Length);
            if (!placedItems.Contains(rand))
            {
                GameObject item = GameObject.Instantiate(items[rand]);
                item.transform.position = new Vector2(x + 0.5f, -y + 2.0f);
                itemCount -= 1;
                placedItems.Add(rand);
            }

        }

        setupComplete = true;

    }
    public IEnumerator FinalizeSetup()
    {
        yield return null;
        Debug.Log("ItemFinalize Start");

        InventorySystem inventorySystem = GameObject.FindObjectOfType<InventorySystem>();

        foreach (Pickupable pickupable in GameObject.FindObjectsOfType<Pickupable>())
        {
            pickupable.setInventorySystem(inventorySystem);
        }

        finalizedComplete = true;
    }
}

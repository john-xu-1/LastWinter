using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSetup : Setup
{
    public GameObject playerPrefab;
    public UnityEngine.Tilemaps.Tilemap collsionMap;
    public InventorySystem inventorySystemPrefab;

    bool setupComplete = false;
    bool finalizedComplete = false;
    public bool SetupComplete() { return setupComplete; }
    public bool FinalizedComplete() { return finalizedComplete; }

    public IEnumerator FinalizeSetup()
    {
        throw new System.NotImplementedException();
    }
    public IEnumerator InitializeSetup(Clingo_02.AnswerSet answerSet, int seed)
    {
        System.Random random = new System.Random(seed);
        foreach (List<string> atom in answerSet.Value["piece"])
        {
            int platformID = 0;
            GameObject prefab = null;
            if (atom[0] == "player")
            {
                prefab = playerPrefab;
                platformID = int.Parse(atom[1]);
            }
            if (platformID > 0)
            {
                //NoiseTerrain.PlatformChunk platform = GameObject.FindObjectOfType<NoiseTerrain.ProceduralMapGenerator>().GetPlatform(platformID);
                //Vector2Int ground = platform.groundTiles[random.Next(0, platform.groundTiles.Count)];
                Vector2 groundPos = SceneLoader.GetRandomGround(platformID, random);//platform.GetTilePos(ground);
                GameObject player = GameObject.Instantiate(playerPrefab);
                player.transform.position = new Vector2(groundPos.x + 0.5f, groundPos.y/* + 1*/);
                GameObject.FindObjectOfType<GameHandler>().StartGameHandler(player);
                player.GetComponent<HealthPlayer>().GH = GameObject.FindObjectOfType<GameHandler>().gameObject;
            }
        }


        //load inventorySystem
        GameObject.Instantiate(inventorySystemPrefab);

        Debug.Log("PlayerSetup Complete");
        setupComplete = true;
        yield return null;

    }
    public IEnumerator InitializeSetup(List<NoiseTerrain.PlatformChunk> platforms, int seed)
    {
        System.Random random = new System.Random(seed);
        NoiseTerrain.PlatformChunk platform = platforms[random.Next(0, platforms.Count)];
        Vector2Int ground = platform.jumpTiles[random.Next(0, platform.jumpTiles.Count)];
        Vector2Int groundPos = platform.GetTilePos(ground);
        GameObject player = GameObject.Instantiate(playerPrefab);
        player.transform.position = new Vector2(groundPos.x + 0.5f, groundPos.y + 1);
        GameObject.FindObjectOfType<GameHandler>().StartGameHandler(player);
        player.GetComponent<HealthPlayer>().GH = GameObject.FindObjectOfType<GameHandler>().gameObject;

        //load inventorySystem
        GameObject.Instantiate(inventorySystemPrefab);

        Debug.Log("PlayerSetup Complete");
        setupComplete = true;
        yield return null;

    }
    public IEnumerator InitializeSetup(int minX, int maxX, int minY, int maxY, int seed)
    {


        int x = Random.Range(minX, maxX);
        int y = Random.Range(minY, maxY);
        while (!UtilityTilemap.isGround(x, -y, collsionMap))
        {
            x = Random.Range(minX, maxX);
            y = Random.Range(minY, maxY);
        }
        GameObject player = GameObject.Instantiate(playerPrefab);
        player.transform.position = new Vector2(x + 0.5f, -y + 1);
        GameObject.FindObjectOfType<GameHandler>().StartGameHandler(player);
        player.GetComponent<HealthPlayer>().GH = GameObject.FindObjectOfType<GameHandler>().gameObject;

        //load inventorySystem
        GameObject.Instantiate(inventorySystemPrefab);

        Debug.Log("PlayerSetup Complete");
        setupComplete = true;

        yield return null;
    }
}

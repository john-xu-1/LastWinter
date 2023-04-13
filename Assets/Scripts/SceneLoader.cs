using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public bool generateFromFile;
    public DungeonHandler dungeonHandler;
    public NoiseTerrain.ProceduralMapGenerator noiseMapGenerator;

    public GameHandler gameHandler;
    public UISetup uiSetup;
    public CameraSetup cameraSetup;
    public PlayerSetup playerSetup;

    public EnemySetup enemySetup;
    public ItemSetup itemSetup;

    public float progression = 0;

    public int progressionItemsCount = 9;
    public int seed;

    int maxX, minX, maxY, minY;



    void Start()
    {
        seed = noiseMapGenerator.seed;
        StartSetup();
    }

    void StartSetup()
    {
        StartCoroutine(LoadingScreenSetup());
    }
    IEnumerator LoadingScreenSetup()
    {
        Debug.Log("Yielding");
        loadingScreen.SetActive(true);
        loadingScreen.GetComponent<LoadingScreen>().StartCounter();
        yield return null;
        StartCoroutine(TilemapSetup());
    }
    public int loadWorldIndex = 0;
    IEnumerator TilemapSetup()
    {
        //generate new RoomChunk or load from saved
        if (generateFromFile)
        {
            dungeonHandler.MapSetup(loadWorldIndex);
            foreach(NoiseTerrain.Chunk roomChunk in dungeonHandler.chunks)
            {
                NoiseTerrain.ChunkHandler.singlton.AddChunk(roomChunk);
                
            }
            minX = 0;
            maxY = dungeonHandler.worldHeight * dungeonHandler.roomHeight;
            maxX = dungeonHandler.worldWidth * dungeonHandler.roomWidth;
            minY = 0;
            noiseMapGenerator.SetRoomChunk(dungeonHandler.chunks);
            Vector2Int chunkRadius = new Vector2Int(dungeonHandler.worldWidth / 2, dungeonHandler.worldHeight / 2);
            Vector2Int chunkSize = new Vector2Int(dungeonHandler.roomWidth, dungeonHandler.roomHeight);
            Vector2 pos = new Vector2(maxX / 2, -maxY / 2);
            Debug.Log($"minY:{minY} maxX:{maxX} pos:{pos}");
            noiseMapGenerator.SetupProceduralMapGenerator(chunkRadius, chunkSize, pos, true);
            while (!noiseMapGenerator.setupComplete)
            {
                yield return null;
            }
        }
        else
        {
            noiseMapGenerator.GenerateMap(noiseMapGenerator.seed);
            while (!noiseMapGenerator.setupComplete)
            {
                yield return null;
            }
            noiseMapGenerator.SetRoomChunk();
            minX = noiseMapGenerator.minX;
            maxX = noiseMapGenerator.maxX;
            minY = noiseMapGenerator.minY;
            maxY = noiseMapGenerator.maxY;
        }

        
        FindObjectOfType<PathFinder>().SetMap(minX, -maxY, maxX, -minY);

        progression += 1 / (float)progressionItemsCount;
        Debug.Log("TilemapSetupComplete");

        StartCoroutine(UISetup());
    }
    IEnumerator UISetup()
    {
        StartCoroutine(uiSetup.InitializeSetup(minX, maxX, minY, maxY, seed));
        while (!uiSetup.SetupComplete())
        {
            yield return null;
        }

        progression += 1 / (float)progressionItemsCount;
        StartCoroutine(CameraSetup());
    }
    IEnumerator CameraSetup()
    {
        yield return null;
        progression += 1 / (float)progressionItemsCount;
        StartCoroutine(AudioSetup());
    }
    IEnumerator AudioSetup()
    {
        yield return null;
        progression += 1 / (float)progressionItemsCount;
        StartCoroutine(EnvironmentSetup());
    }
    IEnumerator EnvironmentSetup()
    {
        while (!noiseMapGenerator.platformSetupComplete) yield return null;
        LightingLevelSetup lighting = FindObjectOfType<LightingLevelSetup>();
        //lighting.setupLighting(minX, maxX, minY + 2, maxY - 2,seed);
        lighting.setupLighting(noiseMapGenerator.GetPlatforms(), seed);
        while (!lighting.setupComplete)
        {
            yield return null;
        }
        progression += 1 / (float)progressionItemsCount;
        StartCoroutine(ObstaclesSetup());
    }
    IEnumerator ObstaclesSetup()
    {
        StartCoroutine(noiseMapGenerator.GenerateLocomotionGraph());
        while (noiseMapGenerator.generatingLocomotionGraph)
        {
            yield return null;
        }

        foreach(List<string> atom in GameObject.FindObjectOfType<ASPLocomotionSolver>().GetAnswerset().Value["sink_source"])
        {
            noiseMapGenerator.GenerateLiquid(int.Parse(atom[0]), int.Parse(atom[1]));
        }
        
        progression += 1 / (float)progressionItemsCount;
        StartCoroutine(PickablesSetup());
    }
    IEnumerator PickablesSetup()
    {
        //StartCoroutine(itemSetup.InitializeSetup(noiseMapGenerator.GetPlatforms(), seed));
        StartCoroutine(itemSetup.InitializeSetup(GameObject.FindObjectOfType<ASPLocomotionSolver>().GetAnswerset(), seed));
        while (!itemSetup.SetupComplete())
        {
            yield return null;
        }
        progression += 1 / (float)progressionItemsCount;
        StartCoroutine(EnemiesSetup());
    }
    IEnumerator EnemiesSetup()
    {
        StartCoroutine(enemySetup.InitializeSetup(GameObject.FindObjectOfType<ASPLocomotionSolver>().GetAnswerset(), seed, false));
        while (!enemySetup.SetupComplete())
        {
            yield return null;
        }
        progression += 1 / (float)progressionItemsCount;
        StartCoroutine(PlayerSetup());
    }

    IEnumerator PlayerSetup()
    {
        //StartCoroutine(playerSetup.InitializeSetup(minX, maxX, minY + 2, maxY - 2, seed));
        StartCoroutine(playerSetup.InitializeSetup(GameObject.FindObjectOfType<ASPLocomotionSolver>().GetAnswerset(), seed));
        while (!playerSetup.SetupComplete())
        {
            yield return null;
        }
        progression += 1 / (float)progressionItemsCount;
        StartCoroutine(GameStartSetup());
    }

    IEnumerator GameStartSetup()
    {
        StartCoroutine(uiSetup.FinalizeSetup());
        while (!uiSetup.FinalizedComplete())
        {
            yield return null;
        }

        StartCoroutine(cameraSetup.FinalizeSetup());
        while (!cameraSetup.FinalizedComplete())
        {
            yield return null;
        }

        StartCoroutine(itemSetup.FinalizeSetup());
        while (!itemSetup.FinalizedComplete())
        {
            yield return null;
        }

        loadingScreen.SetActive(false);

    }

}
public interface Setup
{
    bool SetupComplete();
    bool FinalizedComplete();
    IEnumerator InitializeSetup(int minX, int maxX, int minY, int maxY, int seed);
    IEnumerator FinalizeSetup();
}


[System.Serializable]
public class UISetup : Setup
{
    bool setupComplete = false;
    bool finalizedComplete = false;
    public bool SetupComplete() { return setupComplete; }
    public bool FinalizedComplete() { return finalizedComplete; }
    public IEnumerator InitializeSetup(int minX, int maxX, int minY, int maxY, int seed)
    {
        //setup the world's width and height
        setupComplete = true;

        yield return null;
    }

    public GameObject[] FinalizeUIGameObjects;
    public IEnumerator FinalizeSetup()
    {

        foreach (GameObject UI in FinalizeUIGameObjects)
        {
            UI.SendMessage("FinalizeSetup");
        }
        finalizedComplete = true;

        yield return null;
    }
   
}

[System.Serializable]
public class CameraSetup : Setup
{
    bool setupComplete = false;
    bool finalizedComplete = false;
    public bool SetupComplete() { return setupComplete; }
    public bool FinalizedComplete() { return finalizedComplete; }
    public IEnumerator InitializeSetup(int minX, int maxX, int minY, int maxY, int seed)
    {
        setupComplete = true;
        yield return null;
    }
    public CameraController camController;
    public IEnumerator FinalizeSetup()
    {
        camController.active = true;
        finalizedComplete = true;
        yield return null;
    }
}


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
            if(platformID > 0)
            {
                NoiseTerrain.PlatformChunk platform = GameObject.FindObjectOfType<NoiseTerrain.ProceduralMapGenerator>().GetPlatform(platformID);
                Vector2Int ground = platform.groundTiles[random.Next(0, platform.groundTiles.Count)];
                Vector2Int groundPos = platform.GetTilePos(ground);
                GameObject player = GameObject.Instantiate(playerPrefab);
                player.transform.position = new Vector2(groundPos.x + 0.5f, groundPos.y + 1);
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
        Vector2Int ground = platform.groundTiles[random.Next(0, platform.groundTiles.Count)];
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

[System.Serializable]
public class EnemySetup : Setup
{
    public UnityEngine.Tilemaps.Tilemap collsionMap;
    public GameObject[] enemies;
    public int enemyCount = 10;
    public NoiseTerrain.ProceduralMapGenerator map;

    bool setupComplete = false;
    bool finalizedComplete = false;
    public bool SetupComplete() { return setupComplete; }
    public bool FinalizedComplete() { return finalizedComplete; }

    public int attempts = 10;
    public IEnumerator InitializeSetup(Clingo_02.AnswerSet answerSet, int seed, bool setActive)
    {
        yield return null;
        System.Random random = new System.Random(seed);
        foreach (List<string> atom in answerSet.Value["piece"])
        {
            int platformID = 0;
            GameObject prefab = null;
            if (atom[0] == "missile_launcher")
            {
                prefab = enemies[0];
                platformID = int.Parse(atom[1]);
            }
            else if (atom[0] == "rolla_boi")
            {
                prefab = enemies[1];
                platformID = int.Parse(atom[1]);
            }
            else if (atom[0] == "shotgun_boi")
            {
                prefab = enemies[2];
                platformID = int.Parse(atom[1]);
            }
            else if (atom[0] == "bounce_boi")
            {
                prefab = enemies[3];
                platformID = int.Parse(atom[1]);
            }
            if (platformID > 0)
            {
                GameObject enemy = GameObject.Instantiate(prefab);
                NoiseTerrain.PlatformChunk platform = GameObject.FindObjectOfType<NoiseTerrain.ProceduralMapGenerator>().GetPlatform(platformID);
                Vector2Int ground = platform.groundTiles[random.Next(0, platform.groundTiles.Count)];
                Vector2Int groundPos = platform.GetTilePos(ground);
                enemy.transform.position = new Vector2(groundPos.x + 0.5f, groundPos.y + 1.6f);
                NoiseTerrain.Chunk myChunk = NoiseTerrain.ChunkHandler.singlton.GetChunk(enemy.transform.position);
                enemy.GetComponent<ChunkObjectEnemy>().mychunk = myChunk;
                myChunk.AddChunkObject(enemy.GetComponent<ChunkObjectEnemy>());

                enemy.SetActive(setActive);
            }

        }
        
        setupComplete = true;
    }
    public IEnumerator InitializeSetup(List<NoiseTerrain.PlatformChunk> platforms, int seed, bool setActive)
    {
        yield return null;
        System.Random random = new System.Random(seed);
        List<int> usedPlatforms = new List<int>();
        int remainingEnemies = enemyCount;
        while (remainingEnemies > 0)
        {

            int rand = random.Next(0, enemies.Length);
            remainingEnemies -= 1;

            GameObject enemy = GameObject.Instantiate(enemies[rand]);
            NoiseTerrain.PlatformChunk platform = platforms[random.Next(0, platforms.Count)];
            Vector2Int ground = platform.groundTiles[random.Next(0, platform.groundTiles.Count)];
            Vector2Int groundPos = platform.GetTilePos(ground);
            enemy.transform.position = new Vector2(groundPos.x + 0.5f, groundPos.y + 1.6f);
            NoiseTerrain.Chunk myChunk = NoiseTerrain.ChunkHandler.singlton.GetChunk(enemy.transform.position);
            enemy.GetComponent<ChunkObjectEnemy>().mychunk = myChunk;
            myChunk.AddChunkObject(enemy.GetComponent<ChunkObjectEnemy>());

            enemy.SetActive(setActive);
        }
        setupComplete = true;
    }
    public IEnumerator InitializeSetup(int minX, int maxX, int minY, int maxY, int seed)
    {
        System.Random random = new System.Random(seed);
        
        int remainingEnemies = enemyCount;
        while (remainingEnemies > 0)
        {
            int x = random.Next(minX, maxX);
            int y = random.Next(minY, maxY);
            int attemptCount = 0;
            while (attemptCount < attempts && !UtilityTilemap.isGround(x, -y, collsionMap))
            {
                x = random.Next(minX, maxX);
                y = random.Next(minY, maxY);
                attemptCount += 1;
            }
            if(attemptCount < attempts)
            {
                int rand = random.Next(0, enemies.Length);
                GameObject enemy = GameObject.Instantiate(enemies[rand]);
                enemy.transform.position = new Vector2(x + 0.5f, -y + 1.6f);
                
            }
            remainingEnemies -= 1;


            yield return null;
        }
        setupComplete = true;
    }
    public IEnumerator FinalizeSetup()
    {
        throw new System.NotImplementedException();
    }
}

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
        foreach (List<string> atom in answerSet.Value["piece"]){
            int platformID = 0;
            GameObject prefab = null;
            if(atom[0] == "drone_controller")
            {
                prefab = items[0];
                platformID = int.Parse(atom[1]);
            }else if (atom[0] == "magnetized_shifter")
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
            if( platformID > 0)
            {
                NoiseTerrain.PlatformChunk platform = GameObject.FindObjectOfType<NoiseTerrain.ProceduralMapGenerator>().GetPlatform(platformID);
                Vector2Int ground = platform.groundTiles[random.Next(0, platform.groundTiles.Count)];
                Vector2Int groundPos = platform.GetTilePos(ground);
                GameObject item = GameObject.Instantiate(prefab);
                item.transform.position = new Vector2(groundPos.x + 0.5f, groundPos.y + 2);
            }
            
        }
        setupComplete = true;
    }

    public IEnumerator InitializeSetup(List<NoiseTerrain.PlatformChunk> platforms, int seed)
    {
        yield return null;
        System.Random random = new System.Random(seed);
        List<int> usedPlatforms = new List<int>();
        while(itemCount > 0 && placedItems.Count < items.Length)
        {

            int rand = random.Next(0, items.Length);
            if (!placedItems.Contains(rand))
            {
                itemCount -= 1;
                placedItems.Add(rand);
                GameObject item = GameObject.Instantiate(items[rand]);
                NoiseTerrain.PlatformChunk platform = platforms[random.Next(0, platforms.Count)];
                Vector2Int ground = platform.groundTiles[random.Next(0, platform.groundTiles.Count)];
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
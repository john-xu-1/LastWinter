using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public GameObject LoadingScreen;
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
        StartSetup();
    }

    void StartSetup()
    {
        StartCoroutine(LoadingScreenSetup());
    }
    IEnumerator LoadingScreenSetup()
    {
        Debug.Log("Yielding");
        yield return null;
        StartCoroutine(TilemapSetup());
    }
    IEnumerator TilemapSetup()
    {
        //generate new RoomChunk or load from saved
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
        FindObjectOfType<PathFinder>().SetMap(minX, -maxY, maxX, -minY);

        progression += 1 / (float)progressionItemsCount;
        Debug.Log("TilemapSetupComplete");

        StartCoroutine(UISetup());
    }
    IEnumerator UISetup()
    {
        StartCoroutine(uiSetup.InitializeSetup(minX, maxX, minY, maxY, seed));
        while (!uiSetup.setupComplete)
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
        yield return null;
        progression += 1 / (float)progressionItemsCount;
        StartCoroutine(PickablesSetup());
    }
    IEnumerator PickablesSetup()
    {
        StartCoroutine(itemSetup.InitializeSetup(minX, maxX, minY + 2, maxY - 2, seed));
        while (!itemSetup.setupComplete)
        {
            yield return null;
        }
        progression += 1 / (float)progressionItemsCount;
        StartCoroutine(EnemiesSetup());
    }
    IEnumerator EnemiesSetup()
    {
        StartCoroutine(enemySetup.InitializeSetup(minX, maxX, minY, maxY, seed));
        while (!enemySetup.setupComplete)
        {
            yield return null;
        }
        progression += 1 / (float)progressionItemsCount;
        StartCoroutine(PlayerSetup());
    }

    IEnumerator PlayerSetup()
    {
        StartCoroutine(playerSetup.InitializeSetup(minX, maxX, minY + 2, maxY - 2, seed));
        while (!playerSetup.setupComplete)
        {
            yield return null;
        }
        progression += 1 / (float)progressionItemsCount;
        StartCoroutine(GameStartSetup());
    }

    IEnumerator GameStartSetup()
    {
        StartCoroutine(uiSetup.FinalizeSetup());
        while (!uiSetup.finalizedComplete)
        {
            yield return null;
        }

        StartCoroutine(cameraSetup.FinalizeSetup());
        while (!cameraSetup.finalizedComplete)
        {
            yield return null;
        }

        StartCoroutine(itemSetup.FinalizeSetup());
        while (!itemSetup.finalizedComplete)
        {
            yield return null;
        }


    }

}
public abstract class Setup
{
    public bool setupComplete;
    public bool finalizedComplete;
    public abstract IEnumerator InitializeSetup(int minX, int maxX, int minY, int maxY, int seed);
    public abstract IEnumerator FinalizeSetup();
}


[System.Serializable]
public class UISetup : Setup
{
    public override IEnumerator InitializeSetup(int minX, int maxX, int minY, int maxY, int seed)
    {
        //setup the world's width and height
        setupComplete = true;

        yield return null;
    }

    public GameObject[] FinalizeUIGameObjects;
    public override IEnumerator FinalizeSetup()
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
    public override IEnumerator InitializeSetup(int minX, int maxX, int minY, int maxY, int seed)
    {
        setupComplete = true;
        yield return null;
    }
    public CameraController camController;
    public override IEnumerator FinalizeSetup()
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

    public override IEnumerator FinalizeSetup()
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator InitializeSetup(int minX, int maxX, int minY, int maxY, int seed)
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

    public int attempts = 10;
    
    public override IEnumerator InitializeSetup(int minX, int maxX, int minY, int maxY, int seed)
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
    public override IEnumerator FinalizeSetup()
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




    public override IEnumerator InitializeSetup(int minX, int maxX, int minY, int maxY, int seed)
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
                GameObject enemy = GameObject.Instantiate(items[rand]);
                enemy.transform.position = new Vector2(x + 0.5f, -y + 2.0f);
                itemCount -= 1;
                placedItems.Add(rand);
            }



        }

        setupComplete = true;


    }
    public override IEnumerator FinalizeSetup()
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
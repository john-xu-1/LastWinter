using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public GameObject LoadingScreen;
    public DungeonHandler dungeonHandler;

    public GameHandler gameHandler;
    public PlayerSetup playerSetup;

    public EnemySetup enemySetup;
    public ItemSetup itemSetup;

    public float progression = 0;

    public int progressionItemsCount = 9;


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
        dungeonHandler.MapSetup(dungeonHandler.worlds.BuiltWorlds[0]);
        while (!dungeonHandler.setupComplete)
        {
            yield return null;
        }
        progression += 1 / (float)progressionItemsCount;
        Debug.Log("TilemapSetupComplete");
        StartCoroutine(UISetup());
    }
    IEnumerator UISetup()
    {
        yield return null;
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
        LightingLevelSetup lighting = FindObjectOfType<LightingLevelSetup>();
        lighting.setupLighting(200, 100);
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
        itemSetup.Setup();
        while (!itemSetup.setupComplete)
        {
            yield return null;
        }
        progression += 1 / (float)progressionItemsCount;
        StartCoroutine(EnemiesSetup());
    }
    IEnumerator EnemiesSetup()
    {
        enemySetup.Setup();
        while (!enemySetup.setupComplete)
        {
            yield return null;
        }
        progression += 1 / (float)progressionItemsCount;
        StartCoroutine(PlayerSetup());
    }

    IEnumerator PlayerSetup()
    {
        playerSetup.Setup();
        while (!playerSetup.setupComplete)
        {
            yield return null;
        }
        progression += 1 / (float)progressionItemsCount;
        StartCoroutine(GameStartSetup());
    }

    IEnumerator GameStartSetup()
    {
        yield return null;
    }

}

[System.Serializable]
public class PlayerSetup
{
    public bool setupComplete = false;
    public GameObject playerPrefab;
    public UnityEngine.Tilemaps.Tilemap collsionMap;
    public void Setup()
    {
        int minX = 2;
        int minY = 2;
        int maxX = 200;
        int maxY = 100;

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
        setupComplete = true;
    }
}

[System.Serializable]
public class EnemySetup
{
    public bool setupComplete = false;
    public UnityEngine.Tilemaps.Tilemap collsionMap;
    public GameObject[] enemies;
    public int enemyCount = 10;
    public void Setup()
    {
        int minX = 2;
        int minY = 2;
        int maxX = 200;
        int maxY = 100;

        while (enemyCount > 0)
        {
            int x = Random.Range(minX, maxX);
            int y = Random.Range(minY, maxY);
            while (!UtilityTilemap.isGround(x, -y, collsionMap))
            {
                x = Random.Range(minX, maxX);
                y = Random.Range(minY, maxY);
            }
            int rand = Random.Range(0, enemies.Length);
            GameObject enemy = GameObject.Instantiate(enemies[rand]);
            enemy.transform.position = new Vector2(x + 0.5f, -y + 1.6f);
            enemyCount -= 1;
        }
        setupComplete = true;
    }
}

[System.Serializable]
public class ItemSetup
{
    public bool setupComplete = false;
    public UnityEngine.Tilemaps.Tilemap collsionMap;
    public GameObject[] items;
    public int itemCount = 5;

    public List<int> placedItems = new List<int>();
    public void Setup()
    {
        int minX = 2;
        int minY = 2;
        int maxX = 200;
        int maxY = 100;

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
}
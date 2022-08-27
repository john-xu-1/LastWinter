using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public GameObject LoadingScreen;
    public DungeonHandler dungeonHandler;

    public GameHandler gameHandler;
    public UISetup uiSetup;
    public CameraSetup cameraSetup;
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
        uiSetup.InitializeSetup();
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
        itemSetup.InitializeSetup();
        while (!itemSetup.setupComplete)
        {
            yield return null;
        }
        progression += 1 / (float)progressionItemsCount;
        StartCoroutine(EnemiesSetup());
    }
    IEnumerator EnemiesSetup()
    {
        enemySetup.InitializeSetup();
        while (!enemySetup.setupComplete)
        {
            yield return null;
        }
        progression += 1 / (float)progressionItemsCount;
        StartCoroutine(PlayerSetup());
    }

    IEnumerator PlayerSetup()
    {
        playerSetup.InitializeSetup();
        while (!playerSetup.setupComplete)
        {
            yield return null;
        }
        progression += 1 / (float)progressionItemsCount;
        StartCoroutine(GameStartSetup());
    }

    IEnumerator GameStartSetup()
    {
        uiSetup.FinalizeSetup();
        while (!uiSetup.finalizedComplete)
        {
            yield return null;
        }

        cameraSetup.FinalizeSetup();
        while (!cameraSetup.finalizedComplete)
        {
            yield return null;
        }

        
    }

}
public abstract class Setup
{
    public bool setupComplete;
    public bool finalizedComplete;
    public abstract void InitializeSetup();
    public abstract void FinalizeSetup();
}


[System.Serializable]
public class UISetup : Setup
{
    public override void InitializeSetup()
    {
        //setup the world's width and height
        setupComplete = true;
    }

    public GameObject[] FinalizeUIGameObjects;
    public override void FinalizeSetup()
    {
        foreach (GameObject UI in FinalizeUIGameObjects)
        {
            UI.SendMessage("FinalizeSetup");
        }
        finalizedComplete = true;
    }
}

[System.Serializable]
public class CameraSetup : Setup
{
    public override void InitializeSetup()
    {
        setupComplete = true;
    }
    public CameraController camController;
    public override void FinalizeSetup()
    {
        camController.active = true;
        finalizedComplete = true;
    }
}


[System.Serializable]
public class PlayerSetup : Setup
{
    public GameObject playerPrefab;
    public UnityEngine.Tilemaps.Tilemap collsionMap;

    public override void FinalizeSetup()
    {
        throw new System.NotImplementedException();
    }

    public override void InitializeSetup()
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
public class EnemySetup : Setup
{
    public UnityEngine.Tilemaps.Tilemap collsionMap;
    public GameObject[] enemies;
    public int enemyCount = 10;
    public override void InitializeSetup()
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
    public override void FinalizeSetup()
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
    public override void InitializeSetup()
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
    public override void FinalizeSetup()
    {
        throw new System.NotImplementedException();
    }
}
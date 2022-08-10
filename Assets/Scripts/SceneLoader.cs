using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public GameObject LoadingScreen;
    public DungeonHandler dungeonHandler;
    public PlayerSetup playerSetup;
    public EnemySetup enemySetup;
    
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
        Debug.Log("TilemapSetupComplete");
        StartCoroutine(UISetup());
    }
    IEnumerator UISetup()
    {
        yield return null;
        StartCoroutine(CameraSetup());
    }
    IEnumerator CameraSetup()
    {
        yield return null;
        StartCoroutine(AudioSetup());
    }
    IEnumerator AudioSetup()
    {
        yield return null;
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
        StartCoroutine(ObstaclesSetup());
    }
    IEnumerator ObstaclesSetup()
    {
        yield return null;
        StartCoroutine(PickablesSetup());
    }
    IEnumerator PickablesSetup()
    {
        yield return null;
        StartCoroutine(EnemiesSetup());
    }
    IEnumerator EnemiesSetup()
    {
        enemySetup.Setup();
        while (!enemySetup.setupComplete)
        {
            yield return null;
        }
        StartCoroutine(PlayerSetup());
    }

    IEnumerator PlayerSetup()
    {
        playerSetup.Setup();
        while (!playerSetup.setupComplete)
        {
            yield return null;
        }
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
        int minX = 1;
        int minY = 1;
        int maxX = 200;
        int maxY = 100;

        int x = Random.Range(minX, maxX);
        int y = Random.Range(minY, maxY);
        while(UtilityTilemap.isGround(x, -y, collsionMap))
        {
            x = Random.Range(minX, maxX);
            y = Random.Range(minY, maxY);
        }
        GameObject player = GameObject.Instantiate(playerPrefab);
        player.transform.position = new Vector2(x, -y);
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
        int minX = 1;
        int minY = 1;
        int maxX = 200;
        int maxY = 100;

        while(enemyCount > 0)
        {
            int x = Random.Range(minX, maxX);
            int y = Random.Range(minY, maxY);
            while (UtilityTilemap.isGround(x, -y, collsionMap))
            {
                x = Random.Range(minX, maxX);
                y = Random.Range(minY, maxY);
            }
            int rand = Random.Range(0, enemies.Length);
            GameObject enemy = GameObject.Instantiate(enemies[rand]);
            enemy.transform.position = new Vector2(x, -y);
            enemyCount -= 1;
        }
        setupComplete = true;
    }
}
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
    public Teleporter teleporterPrefab;
    public Exit exitPrefab;
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
        System.Random random = new System.Random(seed);
        foreach (List<string> atom in GameObject.FindObjectOfType<ASPLocomotionSolver>().GetAnswerset().Value["teleporter"]){
            Teleporter teleporter = Instantiate(teleporterPrefab);
            int nodeID = int.Parse(atom[0]);
            int teleporterID = int.Parse(atom[1]);
            teleporter.teleporterID = teleporterID;

            //NoiseTerrain.PlatformChunk platform = GameObject.FindObjectOfType<NoiseTerrain.ProceduralMapGenerator>().GetPlatform(nodeID);
            //int randomIndex = random.Next(0,platform.groundTiles.Count);

            Vector2 ground = GetRandomGround(nodeID, random);// platform.GetTilePos(platform.groundTiles[randomIndex]);
            teleporter.transform.position = new Vector3(ground.x + 0.5f, ground.y, 0);
        }

        foreach(List<string> atom in GameObject.FindObjectOfType<ASPLocomotionSolver>().GetAnswerset().Value["exit"])
        {
            int nodeID = int.Parse(atom[0]);
            int sceneID = int.Parse(atom[1]);
            Exit exit = Instantiate(exitPrefab);
            exit.exitSceneIndex = sceneID;
            Vector2 ground = GetRandomGround(nodeID, random);
            exit.transform.position = new Vector3(ground.x + 0.5f, ground.y +0.5f, 0);
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

    public static Vector2 GetRandomGround(int nodeID, System.Random random)
    {
        NoiseTerrain.PlatformChunk platform = GameObject.FindObjectOfType<NoiseTerrain.ProceduralMapGenerator>().GetPlatform(nodeID);
        int randomIndex = random.Next(0, platform.jumpTiles.Count);
        return platform.GetTilePos(platform.jumpTiles[randomIndex]);
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






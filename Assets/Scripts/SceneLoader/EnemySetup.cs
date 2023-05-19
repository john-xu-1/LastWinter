using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                //NoiseTerrain.PlatformChunk platform = GameObject.FindObjectOfType<NoiseTerrain.ProceduralMapGenerator>().GetPlatform(platformID);
                //Vector2Int ground = platform.groundTiles[random.Next(0, platform.groundTiles.Count)];
                Vector2 groundPos = SceneLoader.GetRandomGround(platformID, random); //platform.GetTilePos(ground);
                enemy.transform.position = new Vector2(groundPos.x + 0.5f, groundPos.y);
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
            Vector2Int ground = platform.jumpTiles[random.Next(0, platform.jumpTiles.Count)];
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
            if (attemptCount < attempts)
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


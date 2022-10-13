using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoiseTerrain
{
    public class Chunk 
    {
        public Vector2Int chunkID;
        public LightingLevelSetup.LightingChunk<GameObject> lightingChunk;
        public List<EnemyBehaviorBase> enemies = new List<EnemyBehaviorBase>();

        public void ClearChunk(LightingLevelSetup lighting)
        {
            lighting.ReturnLights(lightingChunk);
            enemies.Clear();
            ProceduralMapGenerator mapGenerator = GameObject.FindObjectOfType<ProceduralMapGenerator>();
            foreach(EnemyBehaviorBase enemy in GameObject.FindObjectsOfType<EnemyBehaviorBase>())
            {
                if(chunkID == mapGenerator.GetChunkID(enemy.transform.position))
                {
                    enemies.Add(enemy);
                    enemy.gameObject.SetActive(false);
                }
            }
        }

        public void LoadChunk(LightingLevelSetup lighting)
        {
            lighting.setupLighting(lightingChunk);
            foreach (EnemyBehaviorBase enemy in enemies)
            {
                enemy.gameObject.SetActive(true);
            }
        }
    }
}
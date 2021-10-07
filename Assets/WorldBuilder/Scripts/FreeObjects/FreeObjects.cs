using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    [CreateAssetMenu(fileName = "FreeObjects", menuName = "WorldBuilder/FreeObjects") ]
    public class FreeObjects : ScriptableObject
    {
        public List<KeyFreeObject> keyFreeObjects;
        public List<EnemyFreeObject> enemyFreeObjects;
        public List<EnvironmentFreeObject> environmentFreeObjects;
        public GameObject GetFreeObjectPrefab(FreeObject.FreeObjectTypes objectType, string variation)
        {
            GameObject prefab = null;
            Debug.Log($"FreeObjects GetFreeObjectPrefab ({objectType} , {variation})");
            if(objectType == FreeObject.FreeObjectTypes.key)
            {
                foreach (FreeObject freeObject in keyFreeObjects)
                {
                    if (variation == freeObject.GetVariation()) prefab = freeObject.gameObject;
                }
            }
            else if(objectType == FreeObject.FreeObjectTypes.enemy)
            {
                foreach(EnemyFreeObject freeObject in enemyFreeObjects)
                {
                    if (variation == freeObject.GetVariation()) prefab = freeObject.gameObject;
                }
            }
            else if(objectType == FreeObject.FreeObjectTypes.environment)
            {
                foreach (EnvironmentFreeObject freeObject in environmentFreeObjects)
                {
                    if (variation == freeObject.GetVariation()) prefab = freeObject.gameObject;
                }
            }
            
            return prefab;
        }
    }
}


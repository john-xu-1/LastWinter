using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    [CreateAssetMenu(fileName = "FreeObjects", menuName = "WorldBuilder/FreeObjects") ]
    public class FreeObjects : ScriptableObject
    {
        public List<KeyFreeObject> freeObjects;
        public GameObject GetFreeObjectPrefab(FreeObject.FreeObjectTypes objectType, string variation)
        {
            GameObject prefab = null;
            Debug.Log($"FreeObjects GetFreeObjectPrefab ({objectType} , {variation})");
            foreach(FreeObject freeObject in freeObjects)
            {
                if (freeObject.FreeObjectType == objectType && variation == freeObject.GetVariation()) prefab = freeObject.gameObject;
            }
            return prefab;
        }
    }
}


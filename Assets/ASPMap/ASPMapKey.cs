using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASPMap
{
    public abstract class ASPMapKey : ScriptableObject
    {
        public string widthKey = "width";
        public string heightKey = "height";
        public string tileKey = "tile";
        public int xIndex = 0;
        public int yIndex = 1;
        public int tileTypeIndex = 2;

    }

    [System.Serializable]
    public class MapObjectKey<T>
    {
        public MapObject<T>[] mapObjects;
        public T this[string key]
        {
            get => FindObject(key);
        }
        T FindObject(string key)
        {
            T obj = default;
            foreach (MapObject<T> mapObject in mapObjects)
            {
                if (key == mapObject.key) obj = mapObject.obj;
            }
            return obj;
        }
    }

    [System.Serializable]
    public class MapObject<T>
    {
        public string key;
        public T obj;
    }
  
}


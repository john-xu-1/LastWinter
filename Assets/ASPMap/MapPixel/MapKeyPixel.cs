using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASPMap
{
    [CreateAssetMenu(fileName = "MapKeyPixel", menuName = "ASP/ASPMap/MapKeyPixel")]
    public class MapKeyPixel : ASPMapKey
    {
        public MapObjectKey<Color> colorDict;
    }
}

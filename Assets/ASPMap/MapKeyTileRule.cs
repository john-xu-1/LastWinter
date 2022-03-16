using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ASPMap
{
    [CreateAssetMenu(fileName = "MapKeyTileRule", menuName = "ASP/ASPMap/MapKeyTileRule")]
    public class MapKeyTileRule : ASPMapKey
    {
        public MapObjectKey<TileBaseTileRules> dict;
    }
}
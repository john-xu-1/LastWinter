using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public CollisionMap CMap;
    public void ConvertMap (string bitmap)
    {
        string[] rows = bitmap.Split('\n');
        int rowCount = 0;
        foreach (string row in rows)
        {
            int colCount = 0;
            foreach (char bit in row)
            {
                CMap.AddCollisionTiles(new Vector2Int(colCount, -rowCount), (int) char.GetNumericValue(bit));
                colCount += 1;
            }
            rowCount += 1;
        }

        CMap.DebugPlaceTiles();
    }   

    public void ConvertMap(Map map)
    {
        foreach(Tile tile in map.area)
        {
            CMap.AddCollisionTiles(new Vector2Int(tile.x, -tile.y), tile.type);
        }
        CMap.DebugPlaceTiles();
    }
    
}

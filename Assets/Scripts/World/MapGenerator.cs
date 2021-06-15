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
        int worldWidth = 0;
        int worldHeight = 0;

        foreach(Tile tile in map.area)
        {
            worldHeight = Mathf.Max(worldHeight, tile.y);
            worldWidth = Mathf.Max(worldWidth, tile.x);

        }

        CMap.newCollisionMap(worldWidth, worldHeight);

        foreach (Tile tile in map.area)
        {
            CMap.AddCollisionTiles(new Vector2Int(tile.x, -tile.y), tile.type);
        }

        CMap.DebugPlaceTiles();


    }

}

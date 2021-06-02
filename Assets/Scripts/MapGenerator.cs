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
        foreach (Tile tile in map.area)
        {
            worldWidth = Mathf.Max(worldWidth, tile.x);
            worldHeight = Mathf.Max(worldHeight, tile.y);
        }
        //print(worldWidth + " x " + worldHeight);
        CMap.newCollisionMap(worldWidth, worldHeight);
        //FindObjectOfType<LevelHandler>().Setup(new Vector2(map.start.x, -map.start.y),map.dimensions.room_width,map.dimensions.room_height);
        FindObjectOfType<LevelHandler>().Setup(map);
        foreach (Tile tile in map.area)
        {
            CMap.AddCollisionTiles(new Vector2Int(tile.x, -tile.y), tile.type);
        }

        //print("Completed Map Json Load");
        CMap.DebugPlaceTiles();


    }

}

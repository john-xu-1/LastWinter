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

    public void ConvertMap(Dictionary<string, List<List<string>>> dict)
    {
        Map map = new Map();
        map.dimensions = GetDimensions(dict);
        int height = map.dimensions.room_count_width*map.dimensions.room_width;
        int width = map.dimensions.room_count_height * map.dimensions.room_height;
        print(width + "x" + height);

        map.area = GetTiles(dict);
        


        
        foreach (List <string> path in dict["path"])
        {
            if(path.Count == 4 && path[3] == "middle")
            {
                float x = float.Parse(path[0]);
                float y = float.Parse(path[1]);
                map.start = new Vector2(x, y);
            }
        }

        ConvertMap(map);
    }

    public int Max(List<List<string>> values)
    {
        int max = int.MinValue;
        foreach(List<string> value in values)
        {
            if(int.Parse(value[0]) > max)
            {
                max = int.Parse(value[0]);
            }
        }
        return max;
    }

    public Dimensions GetDimensions(Dictionary<string, List<List<string>>> world)
    {
        Dimensions dimensions = new Dimensions();
        

        if (world.ContainsKey("dimensions"))
        {
            //dimensions.room_count_width = int.Parse(world["dimensions"]["room_count_width"][0]);
        }
        else
        {
            dimensions.room_width = Max(world["width"]);
            dimensions.room_height = Max(world["height"]);
            dimensions.room_count_width = 1;
            dimensions.room_count_height = 1;
        }
        

        return dimensions;
    }

    public Tile[] GetTiles(Dictionary<string, List<List<string>>> world)
    {
        int height = Max(world["height"]);
        int width = Max(world["width"]);

        Tile[] area = new Tile[height * width];
        int tileIndex = 0;
        foreach (List<string> tile in world["tile"])
        {
            Tile newTile = new Tile();
            newTile.x = int.Parse(tile[0]);
            newTile.y = int.Parse(tile[1]);
            if (tile[2] == "filled") newTile.type = 1;
            else if (tile[2] == "empty") newTile.type = 0;

            area[tileIndex] = newTile;
            tileIndex += 1;
        }

        return area;
    }
}

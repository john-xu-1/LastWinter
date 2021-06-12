using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room
{
    public Vector2Int pos;
    public Dictionary<string, List<List<string>>> rawMap;
    public Map map;
    public bool isDestroyed;
    public CollisionTile[,] mapGrid;
    public List<CollisionTile> tiles = new List<CollisionTile>();

    public Room(Vector2Int pos, Dictionary<string, List<List<string>>> rawMap)
    {
        this.pos = pos;
        SetupRoom(rawMap);
        Debug.Log("mapGrid size: " + map.dimensions.room_width +", " + map.dimensions.room_height);
        mapGrid = new CollisionTile[map.dimensions.room_width, map.dimensions.room_height];
    }
    public void SetupRoom(Dictionary<string, List<List<string>>> rawMap)
    {
        this.rawMap = rawMap;
        map = ConvertMap(rawMap);
    }
    public void BuildRoom(Tilemap tilemap)
    {

    }

    public void DestroyRoom(Tilemap tilemap)
    {
        foreach (Tile tile in map.area)
        {
            int x = tile.x + pos.x * map.dimensions.room_width - 1;
            int y = -tile.y - pos.y * map.dimensions.room_height - 1;
            UtilityTilemap.DestroyTile(tilemap, new Vector3Int(x, y, 0));
        }
    }

    public Map ConvertMap(Dictionary<string, List<List<string>>> dict)
    {
        Map map = new Map();
        map.dimensions = UtilityBuildWorld.GetDimensions(dict);
        int width = map.dimensions.room_count_width * map.dimensions.room_width;
        int height = map.dimensions.room_count_height * map.dimensions.room_height;
        Debug.Log(width + "x" + height);

        map.area = UtilityBuildWorld.GetTiles(dict);




        foreach (List<string> path in dict["path"])
        {
            if (path.Count == 4 && path[3] == "middle")
            {
                float x = float.Parse(path[0]);
                float y = float.Parse(path[1]);
                map.start = new Vector2(x, y);
            }
        }

        return map;
    }

    
}

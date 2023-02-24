using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WorldBuilder;
public class MapGenerator : MonoBehaviour
{
    [SerializeField] CollisionMap CMap;
    [SerializeField] FreeObjects freeObjects;

    public List<CollisionTile> ConvertMap(Map map, Room room)
    {
        List<CollisionTile> tiles = new List<CollisionTile>();
        foreach (Tile tile in map.area)
        {
            //Debug.Log("CollisionTile location: " + new Vector2Int(tile.x, -tile.y));
            tiles.Add(CMap.AddCollisionTiles(new Vector2Int(tile.x, -tile.y), tile.type, room));
        }
        return tiles;
    }

    public void ConvertMap(Room room)
    {

        room.tiles = ConvertMap(room.map, room);
        //print("Completed Map Json Load");
        CMap.PlaceTiles(room);
    }

    public void RemoveMap(Room room)
    {
        CMap.RemoveTiles(room);
        //need to add RemoveItems(room)
    }
    public List<NoiseTerrain.Chunk> BuildWorldChunks(World world)
    {
        List<NoiseTerrain.Chunk> chunks = new List<NoiseTerrain.Chunk>();
        foreach (Room room in world.GetRooms())
        {
            room.SetupRoom();
            ConvertMap(room.map, room);
            int width = room.map.dimensions.room_width;
            int height = room.map.dimensions.room_height;
            Vector2Int chunkID = new Vector2Int(room.map.dimensions.room_count_width, -room.map.dimensions.room_count_height);
            bool[,] boolMap = new bool[width, height];
            for (int x = 0; x < width; x += 1)
            {
                for(int y = 0; y < height; y += 1)
                {
                    boolMap[x, y] = room.mapGrid[x, y].type == 1 ? true : false;
                }
            }
            NoiseTerrain.Chunk chunk = new NoiseTerrain.Chunk(chunkID, boolMap, FindObjectOfType<NoiseTerrain.ProceduralMapGenerator>());
            chunks.Add(chunk);
        }
        return chunks;
    }

    public void BuildWorld(World world)
    {
        buildingRooms = new List<Room>();
        BuildingRooms = true;
        foreach (Room room in world.GetRooms())
        {
            buildingRooms.Add(room);
        }
    }
    List<Room> buildingRooms;
    public bool BuildingRooms;
    private void Update()
    {
        if (buildingRooms != null && buildingRooms.Count > 0)
        {
            Room room = buildingRooms[0];
            buildingRooms.RemoveAt(0);
            BuildRoom(room);
        }
        else
        {
            BuildingRooms = false;
        }
    }

    public void BuildRoom(Room room)
    {
        room.SetupRoom();
        ConvertMap(room);
        room.BuildRoom(freeObjects);
    }
}
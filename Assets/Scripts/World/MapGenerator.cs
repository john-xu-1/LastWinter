﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WorldBuilder;
public class MapGenerator : MonoBehaviour
{
    public CollisionMap CMap;
    //public void ConvertMap (string bitmap)
    //{
    //    string[] rows = bitmap.Split('\n');
    //    int rowCount = 0;
    //    foreach (string row in rows)
    //    {
    //        int colCount = 0;
    //        foreach (char bit in row)
    //        {
    //            CMap.AddCollisionTiles(new Vector2Int(colCount, -rowCount), (int) char.GetNumericValue(bit));
    //            colCount += 1;
    //        }
    //        rowCount += 1;
    //    }

    //    CMap.DebugPlaceTiles();
    //}

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
        CMap.DebugPlaceTiles(room);
    }

    public void RemoveMap(Room room)
    {
        CMap.RemoveTiles(room);
    }

    public void BuildWorld(World world)
    {

        foreach (Room room in world.GetRooms())
        {
            room.SetupRoom();
            ConvertMap(room);
        }
    }
}
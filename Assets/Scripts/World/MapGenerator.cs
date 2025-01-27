﻿using System.Collections;
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
        CMap.DebugPlaceTiles(room);
    }

    public void RemoveMap(Room room)
    {
        CMap.RemoveTiles(room);
        //need to add RemoveItems(room)
    }

    public void BuildWorld(World world)
    {

        foreach (Room room in world.GetRooms())
        {
            BuildRoom(room);
        }
    }

    public void BuildRoom(Room room)
    {
        room.SetupRoom();
        ConvertMap(room);
        room.BuildRoom(freeObjects);
    }
}
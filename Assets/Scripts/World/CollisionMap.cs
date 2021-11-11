using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using WorldBuilder;
//using Debugger;

public class CollisionMap : MonoBehaviour
{

    private List<Room> rooms = new List<Room>();
    public TileBase Center, Right, Left, Up, Down, TopRight, BottomRight, TopLeft, BottomLeft, LinkersTopRight, LinkersBottomRight, LinkersTopLeft, LinkersBottomLeft;
    public Tilemap tilemap;
    public TileBase waterTop, water;
    public Tilemap waterTilemap;
    public TileBase lavaTop, lava;
    public Tilemap lavaTilemap;
    public TileBase door;
    public Tilemap doorTilemap;
    public TileBase enemyDoor;

    public bool DebugMode(Debugging.Debugger.DebugTypes source) { if (FindObjectOfType<Debugging.Debugger>()) return FindObjectOfType<Debugging.Debugger>().Debug(source); else return debugMode; }
    [SerializeField] private bool debugMode;

    //private int worldWidth, worldHeight;

    //public void newCollisionMap(int worldWidth, int worldHeight)
    //{
    //    this.worldHeight = worldHeight;
    //    this.worldWidth = worldWidth;
    //    mapGrid = new CollisionTile[worldWidth,worldHeight];
    //}

    private List<Vector2Int> doorTiles = new List<Vector2Int>();

    private List<Vector2Int> enemyGateDoorTiles = new List<Vector2Int>();
    private List<Vector2Int> enemyNonGateDoorTiles = new List<Vector2Int>();


    public CollisionTile AddCollisionTiles(Vector2Int pos, int type, Room room)
    {
        Vector2Int offsetPos = new Vector2Int(pos.x + room.pos.x * room.map.dimensions.room_width, pos.y - room.pos.y * room.map.dimensions.room_height);
        CollisionTile tile = new CollisionTile(offsetPos, type);

        room.mapGrid[pos.x - 1, -pos.y - 1] = tile;
        //room.tiles.Add(tile);
        return tile;
    }
    public void BuildMap()
    {

    }

    public bool FindNeighbor(Vector2Int pos, Room room)
    {
        
        int worldHeight = room.map.dimensions.room_height;
        int worldWidth = room.map.dimensions.room_width;
        int y = -pos.y - 1 - room.pos.y * worldHeight;
        int x = pos.x - 1 - room.pos.x * worldWidth;
        //print(y);
        if (y < worldHeight && y >= 0 && x < worldWidth && x >= 0)
        {
            if (room.mapGrid[x, y].type == 1) return true;
            else return false;
        }
        else return true;


    }
    public int[] FindNeighbors(Vector2Int startpos, Room room)
    {
        int[] neighbors = new int[9];
        neighbors[4] = 1;
        for (int i = -1; i <= 1; i += 1)
        {
            for (int j = -1; j <= 1; j += 1)
            {
                Vector2Int pos = new Vector2Int(startpos.x + i, startpos.y + j);
                int x = i + 1;
                int y = -j + 1;
                int index = x + 3 * y;

                if (pos != startpos && FindNeighbor(pos, room))
                {
                    neighbors[index] = 1;
                }
                else
                {
                    neighbors[index] = 0;
                }
            }
        }
        return neighbors;
    }

    public bool FindNeighbor(Vector2Int pos, Room room, int tileType)
    {

        int worldHeight = room.map.dimensions.room_height;
        int worldWidth = room.map.dimensions.room_width;
        int y = -pos.y - 1 - room.pos.y * worldHeight;
        int x = pos.x - 1 - room.pos.x * worldWidth;
        //print(y);
        if (y < worldHeight && y >= 0 && x < worldWidth && x >= 0)
        {
            if (room.mapGrid[x, y].type == tileType) return true;
            else return false;
        }
        else return true;


    }
    public int[] FindNeighbors(Vector2Int startpos, Room room, int tileType)
    {
        int[] neighbors = new int[9];
        neighbors[4] = 1;
        for (int i = -1; i <= 1; i += 1)
        {
            for (int j = -1; j <= 1; j += 1)
            {
                Vector2Int pos = new Vector2Int(startpos.x + i, startpos.y + j);
                int x = i + 1;
                int y = -j + 1;
                int index = x + 3 * y;

                if (pos != startpos && FindNeighbor(pos, room, tileType))
                {
                    neighbors[index] = 1;
                }
                else
                {
                    neighbors[index] = 0;
                }
            }
        }
        return neighbors;
    }

    public void PlaceTiles(Room room)
    {
        
        foreach (CollisionTile tile in room.tiles)
        {
            if (tile.type == 1)
            {

                int[] neighbors = FindNeighbors(tile.pos, room);
                //DEBUG
                if (DebugMode(Debugging.Debugger.DebugTypes.tile_rules))
                {
                    PlaceTile(tilemap, (Vector3Int)tile.pos, Center);
                }
                else if (neighbors[0] == 1 && neighbors[1] == 1 && neighbors[2] == 1 && neighbors[3] == 1 && neighbors[5] == 1 && neighbors[6] == 1 && neighbors[7] == 1 && neighbors[8] == 1)
                    PlaceTile(tilemap, (Vector3Int)tile.pos, Center);
                //RIGHT
                else if (neighbors[0] == 1 && neighbors[1] == 1 && neighbors[3] == 1 && neighbors[5] == 0 && neighbors[6] == 1 && neighbors[7] == 1)
                    PlaceTile(tilemap, (Vector3Int)tile.pos, Right);
                //LEFT
                else if (neighbors[1] == 1 && neighbors[2] == 1 && neighbors[3] == 0 && neighbors[5] == 1 && neighbors[7] == 1 && neighbors[8] == 1)
                    PlaceTile(tilemap, (Vector3Int)tile.pos, Left);
                //UP
                else if (neighbors[1] == 0 && neighbors[3] == 1 && neighbors[5] == 1 && neighbors[6] == 1 && neighbors[7] == 1 && neighbors[8] == 1)
                    PlaceTile(tilemap, (Vector3Int)tile.pos, Up);
                //DOWN
                else if (neighbors[0] == 1 && neighbors[1] == 1 && neighbors[2] == 1 && neighbors[3] == 1 && neighbors[5] == 1 && neighbors[7] == 0)
                    PlaceTile(tilemap, (Vector3Int)tile.pos, Down);
                //TR
                else if (neighbors[0] == 1 && neighbors[1] == 1 && neighbors[2] == 0 && neighbors[3] == 1 && neighbors[5] == 1 && neighbors[6] == 1 && neighbors[7] == 1 && neighbors[8] == 1)
                    PlaceTile(tilemap, (Vector3Int)tile.pos, TopRight);
                //TL
                else if (neighbors[0] == 0 && neighbors[1] == 1 && neighbors[2] == 1 && neighbors[3] == 1 && neighbors[5] == 1 && neighbors[6] == 1 && neighbors[7] == 1 && neighbors[8] == 1)
                    PlaceTile(tilemap, (Vector3Int)tile.pos, TopLeft);
                //BR
                else if (neighbors[0] == 1 && neighbors[1] == 1 && neighbors[2] == 1 && neighbors[3] == 1 && neighbors[5] == 1 && neighbors[6] == 1 && neighbors[7] == 1 && neighbors[8] == 0)
                    PlaceTile(tilemap, (Vector3Int)tile.pos, BottomRight);
                //BL
                else if (neighbors[0] == 1 && neighbors[1] == 1 && neighbors[2] == 1 && neighbors[3] == 1 && neighbors[5] == 1 && neighbors[6] == 0 && neighbors[7] == 1 && neighbors[8] == 1)
                    PlaceTile(tilemap, (Vector3Int)tile.pos, BottomLeft);
                //LTR
                else if (neighbors[1] == 0 && neighbors[2] == 0 && neighbors[3] == 1 && neighbors[5] == 0 && neighbors[6] == 1 && neighbors[7] == 1)
                    PlaceTile(tilemap, (Vector3Int)tile.pos, LinkersTopRight);
                //LTL
                else if (neighbors[0] == 0 && neighbors[1] == 0 && neighbors[3] == 0 && neighbors[5] == 1 && neighbors[7] == 1 && neighbors[8] == 1)
                    PlaceTile(tilemap, (Vector3Int)tile.pos, LinkersTopLeft);
                //LBR
                else if (neighbors[0] == 1 && neighbors[1] == 1 && neighbors[3] == 1 && neighbors[5] == 0 && neighbors[7] == 0 && neighbors[8] == 0)
                    PlaceTile(tilemap, (Vector3Int)tile.pos, LinkersBottomRight);
                //LBL
                else if (neighbors[1] == 1 && neighbors[2] == 1 && neighbors[3] == 0 && neighbors[5] == 1 && neighbors[6] == 0 && neighbors[7] == 0)
                    PlaceTile(tilemap, (Vector3Int)tile.pos, LinkersBottomLeft);
                else
                {
                    print(tile.pos + "Tile placement is missing");
                }
            }
            else if (tile.type == 2) //water
            {
                int[] neighbors = FindNeighbors(tile.pos, room, 2);
                if (DebugMode(Debugging.Debugger.DebugTypes.tile_rules))
                {
                    PlaceTile(waterTilemap, (Vector3Int)tile.pos, water);
                }
                else if (neighbors[1] == 0) PlaceTile(waterTilemap, (Vector3Int)tile.pos, waterTop);
                else PlaceTile(waterTilemap, (Vector3Int)tile.pos, water);
            }
            else if(tile.type == 3) //lava
            {
                int[] neighbors = FindNeighbors(tile.pos, room, tile.type);
                if (DebugMode(Debugging.Debugger.DebugTypes.tile_rules))
                {
                    PlaceTile(lavaTilemap, (Vector3Int)tile.pos, lava);
                }
                else if (neighbors[1] == 0) PlaceTile(lavaTilemap, (Vector3Int)tile.pos, lavaTop);
                else PlaceTile(lavaTilemap, (Vector3Int)tile.pos, lava);
            }
            else if(tile.type == 4) //door
            {
                if (DebugMode(Debugging.Debugger.DebugTypes.tile_rules))
                {
                    PlaceTile(doorTilemap, (Vector3Int)tile.pos, door);
                    doorTiles.Add(tile.pos);
                }
                else
                {
                    PlaceTile(doorTilemap, (Vector3Int)tile.pos, door);
                    doorTiles.Add(tile.pos);
                }
                
            }
            else if(tile.type == 5) //enemy door gated
            {
                if (DebugMode(Debugging.Debugger.DebugTypes.tile_rules))
                {
                    PlaceTile(doorTilemap, (Vector3Int)tile.pos, enemyDoor);
                    enemyGateDoorTiles.Add(tile.pos);
                }
                else
                {
                    PlaceTile(doorTilemap, (Vector3Int)tile.pos, enemyDoor);
                    enemyGateDoorTiles.Add(tile.pos);
                }
            }
            else if (tile.type == 6) //enemy door non-gated 
            {
                if (DebugMode(Debugging.Debugger.DebugTypes.tile_rules))
                {
                    PlaceTile(doorTilemap, (Vector3Int)tile.pos, enemyDoor);
                    enemyNonGateDoorTiles.Add(tile.pos);
                }
                else
                {
                    PlaceTile(doorTilemap, (Vector3Int)tile.pos, enemyDoor);
                    enemyNonGateDoorTiles.Add(tile.pos);
                }
            }


        }
        
    }

    void PlaceTile(Tilemap tilemap, Vector3Int pos, TileBase tileType)
    {
        UtilityTilemap.PlaceTile(tilemap, pos, tileType);
    }

    public void RemoveTiles(Room room)
    {
        room.DestroyRoom(tilemap);
        room.DestroyRoom(waterTilemap);
        room.DestroyRoom(lavaTilemap);
        room.DestroyRoom(doorTilemap);
    }

}




public class CollisionTile
{
    public Vector2Int pos;
    public int type;

    public CollisionTile(Vector2Int pos, int type)
    {
        this.pos = pos;
        this.type = type;
    }


}
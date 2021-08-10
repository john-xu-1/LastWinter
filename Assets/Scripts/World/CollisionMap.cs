using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using WorldBuilder;

public class CollisionMap : MonoBehaviour
{

    
    public TileBase Center, Right, Left, Up, Down, TopRight, BottomRight, TopLeft, BottomLeft, LinkersTopRight, LinkersBottomRight, LinkersTopLeft, LinkersBottomLeft;
    public Tilemap tilemap;

    public CollisionTile AddCollisionTiles(Vector2Int pos, int type, Room room)
    {
        Vector2Int offsetPos = new Vector2Int(pos.x + room.pos.x * room.map.dimensions.room_width, pos.y - room.pos.y * room.map.dimensions.room_height);
        CollisionTile tile = new CollisionTile(offsetPos, type);

        room.mapGrid[pos.x - 1, -pos.y - 1] = tile;
        //room.tiles.Add(tile);
        return tile;
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
            if (room.mapGrid[x, y].type > 0) return true;
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


    public void DebugPlaceTiles(Room room)
    {
        foreach (CollisionTile tile in room.tiles)
        {
            if (tile.type == 1)
            {

                int[] neighbors = FindNeighbors(tile.pos, room);
                if (neighbors[0] == 1 && neighbors[1] == 1 && neighbors[2] == 1 && neighbors[3] == 1 && neighbors[5] == 1 && neighbors[6] == 1 && neighbors[7] == 1 && neighbors[8] == 1)
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
            else if (tile.type == 2)
            {
                UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, Right);
            }
            else
            {

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
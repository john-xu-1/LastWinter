using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollisionMap : MonoBehaviour
{
    private List<CollisionTile> map = new List<CollisionTile>();
    public TileBase Center, Right, Left, Up, Down, TopRight, BottomRight, TopLeft, BottomLeft, LinkersTopRight, LinkersBottomRight, LinkersTopLeft, LinkersBottomLeft;
    public Tilemap tilemap;

    public void AddCollisionTiles(Vector2Int pos, int type)
    {
        CollisionTile tile = new CollisionTile(pos, type);
        map.Add(tile);
    }
    public void BuilMap()
    {

    }

    public bool FindNeighbor(Vector2Int pos)
    {
        foreach (CollisionTile tile in map)
        {
            if (pos == tile.pos && tile.type > 0) return true;
            if (pos == tile.pos && tile.type == 0) return false;
        }
        return true;
    }
    public int[] FindNeighbors(Vector2Int startpos)
    {
        int[] neighbors = new int[9];
        neighbors[4] = 1;
        for (int i = -1; i < 2;  i += 1)
        {
            for (int j = -1; j < 2; j += 1)
            {
                Vector2Int pos = new Vector2Int(startpos.x + i, startpos.y + j);
                int x = i + 1;
                int y = -j + 1;
                int index = x + 3 * y;
                
                if (pos != startpos && FindNeighbor(pos))
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


    public void DebugPlaceTiles()
    {
        foreach (CollisionTile tile in map)
        {
            if(tile.type == 1)
            {
                
                int[] neighbors = FindNeighbors(tile.pos);
                if (neighbors[0] == 1 && neighbors[1]  == 1 && neighbors[2] == 1 && neighbors[3] == 1 && neighbors[5] == 1 && neighbors[6] == 1 && neighbors[7] ==1 && neighbors[8] == 1)
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, Center);
                //RIGHT
                else if (neighbors[0] == 1 && neighbors[1] == 1 && neighbors[3] == 1 && neighbors[5] == 0 && neighbors[6] == 1 && neighbors[7] == 1)
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, Right);
                //LEFT
                else if (neighbors[1] == 1 && neighbors[2] == 1 && neighbors[3] == 0 && neighbors[5] == 1 && neighbors[7] == 1 && neighbors[8] == 1)
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, Left);
                //UP
                else if (neighbors[1] == 0 && neighbors[3] == 1 && neighbors[5] == 1 && neighbors[6] == 1 && neighbors[7] == 1 && neighbors[8] == 1)
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, Up);
                //DOWN
                else if (neighbors[0] == 1 && neighbors[1] == 1 && neighbors[2] == 1 && neighbors[3] == 1 && neighbors[5] == 1 && neighbors[7] == 0)
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, Down);
                //TR
                else if (neighbors[0] == 1 && neighbors[1] == 1 && neighbors[2] == 0 && neighbors[3] == 1 && neighbors[5] == 1 && neighbors[6] == 1 && neighbors[7] == 1 && neighbors[8] == 1)
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, TopRight);
                //TL
                else if (neighbors[0] == 0 && neighbors[1] == 1 && neighbors[2] == 1 && neighbors[3] == 1 && neighbors[5] == 1 && neighbors[6] == 1 && neighbors[7] == 1 && neighbors[8] == 1)
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, TopLeft);
                //BR
                else if (neighbors[0] == 1 && neighbors[1] == 1 && neighbors[2] == 1 && neighbors[3] == 1 && neighbors[5] == 1 && neighbors[6] == 1 && neighbors[7] == 1 && neighbors[8] == 0)
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, BottomRight);
                //BL
                else if (neighbors[0] == 1 && neighbors[1] == 1 && neighbors[2] == 1 && neighbors[3] == 1 && neighbors[5] == 1 && neighbors[6] == 0 && neighbors[7] == 1 && neighbors[8] == 1)
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, BottomLeft);
                //LTR
                else if (neighbors[1] == 0 && neighbors[2] == 0 && neighbors[3] == 1 && neighbors[5] == 0 && neighbors[6] == 1 && neighbors[7] == 1)
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, LinkersTopRight);
                //LTL
                else if (neighbors[0] == 0 && neighbors[1] == 0 && neighbors[3] == 0 && neighbors[5] == 1 && neighbors[7] == 1 && neighbors[8] == 1)
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, LinkersTopLeft);
                //LBR
                else if (neighbors[0] == 1 && neighbors[1] == 1 && neighbors[3] == 1 && neighbors[5] == 0 && neighbors[7] == 0 && neighbors[8] == 0)
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, LinkersBottomRight);
                //LBL
                else if (neighbors[1] == 1 && neighbors[2] == 1 && neighbors[3] == 0 && neighbors[5] == 1 && neighbors[6] == 0 && neighbors[7] == 0)
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, LinkersBottomLeft);
                else
                {
                    print("Tile placement is missing");
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
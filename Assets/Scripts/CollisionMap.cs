using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollisionMap : MonoBehaviour
{
    private List<CollisionTile> map = new List<CollisionTile>();
    public TileBase Center, Right, Left, Up, Down, TopRight, BottomRight, TopLeft, BottomLeft, LinkerTopRight, LinkerBottomRight, LinkerTopLeft, LinkersBottomLeft;
    public Tilemap tilemap;
    

    public void AddCollisionTiles(Vector2Int pos, int type)
    {
        CollisionTile tile = new CollisionTile(pos, type);
        map.Add(tile);
    }

    public void BuildMap()
    {

    }

    public bool FindNeighbor(Vector2Int pos)
    {
        foreach (CollisionTile tile in map)
        {
            if (pos == tile.pos) return true;
        }
        return false;
    }

    public int[] FindNeighbors(Vector2Int startPos)
    {
        int[] neighbors = new int[9];
        neighbors[4] = 1;
        for(int i = -1; i < 2; i += 1)
        {
            for(int j = -1; j < 2; j += 1)
            {
                Vector2Int pos = new Vector2Int(startPos.x + i, startPos.y + j);
                int x = i + 1;
                int y = -j + 1;
                int index = x + 3 * y;
                
                if (pos != startPos && FindNeighbor(pos))
                {
                    neighbors[index] = 1;
                    print(index + " " + pos + " " + startPos + " " + true);
                }
                else if(pos != startPos)
                {
                    neighbors[index] = 0;
                    print(index + " " + pos + " " + startPos + " " + false);
                }
            }
        }
        
        return neighbors;
    }


    public void DebugPlaceTiles()
    {
        BuildMap();
        foreach (CollisionTile tile in map)
        {
            if (tile.type == 1)
            { 
                int[] neighbors = FindNeighbors(tile.pos);
                if(neighbors[1] == 1 && neighbors[3] == 1 && neighbors[5] == 1 && neighbors[7] == 1)
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, Center);
                else if(neighbors[5] == 0)
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, Right);
                else if (neighbors[3] == 0)
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, Left);
                else if (neighbors[1] == 0)
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, Up);
                else if (neighbors[7] == 0)
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, Down);
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
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

    public Vector2 CalculateCentroid()
    {
        int x = 0;
        int y = 0;
        int a = 0;

        foreach(CollisionTile tile in map)
        {
            if (tile.type == 0)
            {
                y += tile.pos.y;
                x += tile.pos.x;
                a += 1;
            }
        }
        return new Vector2(x / a, y / a);
    }

    public bool FindNeighbor(Vector2Int pos)
    {
        foreach (CollisionTile tile in map)
        {
            StartUpCost += 1;
            if (pos == tile.pos && tile.type > 0) return true;
        }
        return false;
    }
    public int StartUpCost = 0;
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
                    //print(index + " " + pos + " " + startpos + " " + true);
                }
                else 
                {
                    neighbors[index] = 0;
                    //print(index + " " + pos + " " + startpos + " " + false);
                }
            }
        }
        return neighbors;
    }


    public void DebugPlaceTiles()
    {
        foreach (CollisionTile tile in map)
        {
            if(tile.type > 0)
            {
                //CENTER
                int[] neighbors = FindNeighbors(tile.pos);
                if (neighbors[0] == 1 && neighbors[1] == 1 && neighbors[2] == 1 && neighbors[3] == 1 && neighbors[5] == 1 && neighbors[6] == 1 && neighbors[7] == 1 && neighbors[8] == 1)
                {
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, Center);
                    //print("C placed at: " + tile.pos + " " + neighbors[0] + ", " + neighbors[1] + ", " + neighbors[2] + ", " + neighbors[3] + ", " + neighbors[4] + ", " + neighbors[5] + ", " + neighbors[6] + ", " + neighbors[7] + ", " + neighbors[8]);
                }
                //RIGHT
                else if (neighbors[0] == 1 && neighbors[1] == 1 && neighbors[3] == 1 && neighbors[5] == 0 && neighbors[6] == 1 && neighbors[7] == 1 )
                {
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, Right);
                    //print("R placed at: " + tile.pos + " " + neighbors[0] + ", " + neighbors[1] + ", " + neighbors[2] + ", " + neighbors[3] + ", " + neighbors[4] + ", " + neighbors[5] + ", " + neighbors[6] + ", " + neighbors[7] + ", " + neighbors[8]);
                }
                //LEFT
                else if ( neighbors[1] == 1 && neighbors[2] == 1 && neighbors[3] == 0 && neighbors[5] == 1  && neighbors[7] == 1 && neighbors[8] == 1)
                {
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, Left);
                    //print("L placed at: " + tile.pos + " " + neighbors[0] + ", " + neighbors[1] + ", " + neighbors[2] + ", " + neighbors[3] + ", " + neighbors[4] + ", " + neighbors[5] + ", " + neighbors[6] + ", " + neighbors[7] + ", " + neighbors[8]);
                }
                //UP
                else if (neighbors[1] == 0 && neighbors[3] == 1 && neighbors[5] == 1 && neighbors[6] == 1 && neighbors[7] == 1 && neighbors[8] == 1)
                {
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, Up);
                    //print("U placed at: " + tile.pos + " " + neighbors[0] + ", " + neighbors[1] + ", " + neighbors[2] + ", " + neighbors[3] + ", " + neighbors[4] + ", " + neighbors[5] + ", " + neighbors[6] + ", " + neighbors[7] + ", " + neighbors[8]);
                }
                //DOWN
                else if (neighbors[0] == 1 && neighbors[1] == 1 && neighbors[2] == 1 && neighbors[3] == 1 && neighbors[5] == 1  && neighbors[7] == 0 )
                {
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, Down);
                    //print("D placed at: " + tile.pos + " " + neighbors[0] + ", " + neighbors[1] + ", " + neighbors[2] + ", " + neighbors[3] + ", " + neighbors[4] + ", " + neighbors[5] + ", " + neighbors[6] + ", " + neighbors[7] + ", " + neighbors[8]);
                }
                //TOP RIGHT
                else if (neighbors[0] == 1 && neighbors[1] == 1 && neighbors[2] == 0 && neighbors[3] == 1 && neighbors[5] == 1 && neighbors[6] == 1 && neighbors[7] == 1 && neighbors[8] == 1)
                {
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, TopRight);
                    //print("TR placed at: " + tile.pos + " " + neighbors[0] + ", " + neighbors[1] + ", " + neighbors[2] + ", " + neighbors[3] + ", " + neighbors[4] + ", " + neighbors[5] + ", " + neighbors[6] + ", " + neighbors[7] + ", " + neighbors[8]);
                }
                //TOP LEFT
                else if (neighbors[0] == 0 && neighbors[1] == 1 && neighbors[2] == 1 && neighbors[3] == 1 && neighbors[5] == 1 && neighbors[6] == 1 && neighbors[7] == 1 && neighbors[8] == 1)
                {
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, TopLeft);
                    //print("TL placed at: " + tile.pos + " " + neighbors[0] + ", " + neighbors[1] + ", " + neighbors[2] + ", " + neighbors[3] + ", " + neighbors[4] + ", " + neighbors[5] + ", " + neighbors[6] + ", " + neighbors[7] + ", " + neighbors[8]);
                }//BOTTOM RIGHT
                else if (neighbors[0] == 1 && neighbors[1] == 1 && neighbors[2] == 1 && neighbors[3] == 1 && neighbors[5] == 1 && neighbors[6] == 1 && neighbors[7] == 1 && neighbors[8] == 0)
                {
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, BottomRight);
                    //print("BR placed at: " + tile.pos + " " + neighbors[0] + ", " + neighbors[1] + ", " + neighbors[2] + ", " + neighbors[3] + ", " + neighbors[4] + ", " + neighbors[5] + ", " + neighbors[6] + ", " + neighbors[7] + ", " + neighbors[8]);
                }
                //bottom left
                else if (neighbors[0] == 1 && neighbors[1] == 1 && neighbors[2] == 1 && neighbors[3] == 1 && neighbors[5] == 1 && neighbors[6] == 0 && neighbors[7] == 1 && neighbors[8] == 1)
                {
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, BottomLeft);
                    //print("BL placed at: " + tile.pos + " " + neighbors[0] + ", " + neighbors[1] + ", " + neighbors[2] + ", " + neighbors[3] + ", " + neighbors[4] + ", " + neighbors[5] + ", " + neighbors[6] + ", " + neighbors[7] + ", " + neighbors[8]);
                }
                //Linker top Right
                else if ( neighbors[1] == 0 && neighbors[2] == 0 && neighbors[3] == 1 && neighbors[5] == 0 && neighbors[6] == 1 && neighbors[7] == 1)
                {
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, LinkersTopRight);
                    //print("LTR placed at: " + tile.pos + " " + neighbors[0] + ", " + neighbors[1] + ", " + neighbors[2] + ", " + neighbors[3] + ", " + neighbors[4] + ", " + neighbors[5] + ", " + neighbors[6] + ", " + neighbors[7] + ", " + neighbors[8]);
                }
                //Likner top left
                else if (neighbors[0] == 0 && neighbors[1] == 0 && neighbors[3] == 0 && neighbors[5] == 1 && neighbors[7] == 1 && neighbors[8] == 1)
                {
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, LinkersTopLeft);
                    //print("LTL placed at: " + tile.pos + " " + neighbors[0] + ", " + neighbors[1] + ", " + neighbors[2] + ", " + neighbors[3] + ", " + neighbors[4] + ", " + neighbors[5] + ", " + neighbors[6] + ", " + neighbors[7] + ", " + neighbors[8]);
                }
                //linker bottom right
                else if (neighbors[0] == 1 && neighbors[1] == 1 && neighbors[3] == 1 && neighbors[5] == 0 && neighbors[7] == 0 && neighbors[8] == 0)
                {
                    //print("LBR placed at: " + tile.pos + " " + neighbors[0] + ", " + neighbors[1] + ", " + neighbors[2] + ", " + neighbors[3] + ", " + neighbors[4] + ", " + neighbors[5] + ", " + neighbors[6] + ", " + neighbors[7] + ", " + neighbors[8]);
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, LinkersBottomRight);
                }
                //linker bottom left
                else if (neighbors[1] == 1 && neighbors[2] == 1 && neighbors[3] == 0 && neighbors[5] == 1 && neighbors[6] == 0 && neighbors[7] == 0)
                {
                    UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, LinkersBottomLeft);
                    //print("LBL placed at: " + tile.pos + " " + neighbors[0] + ", " + neighbors[1] + ", " + neighbors[2] + ", " + neighbors[3] + ", " + neighbors[4] + ", " + neighbors[5] + ", " + neighbors[6] + ", " + neighbors[7] + ", " + neighbors[8]);

                }
                //missing
                else
                {
                    print("missing tile at: " + tile.pos + neighbors[0] + ", " + neighbors[1] + ", " + neighbors[2] + ", " + neighbors[3] + ", " + neighbors[4] + ", " + neighbors[5] + ", " + neighbors[6] + ", " + neighbors[7] + ", " + neighbors[8]);
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
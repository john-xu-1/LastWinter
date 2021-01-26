using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollisionMap : MonoBehaviour
{
    private List<CollisionTile> map = new List<CollisionTile>();
    public TileBase Center, Right, Left, Up, Down, TopRight, BottomRight, TopLeft, BottomLeft;
    public Tilemap tilemap;

    public void AddCollisionTiles(Vector2Int pos, int type)
    {
        CollisionTile tile = new CollisionTile(pos, type);
        map.Add(tile);
    }

    public void DebugPlaceTiles()
    {
        foreach (CollisionTile tile in map)
        {
            if(tile.type == 1)
            {
                UtilityTilemap.PlaceTile(tilemap, (Vector3Int)tile.pos, Center);
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
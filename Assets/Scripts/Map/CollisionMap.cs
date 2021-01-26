using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollisionMap : MonoBehaviour
{
    private List<CollisionTile> map = new List<CollisionTile>();
    private int gridWidthMin, gridWidthMax, gridHeightMin, gridHeightMax;
    private CollisionTile[,] mapGrid;
    public TileBase Center, Right, Left, Up, Down, TopRight, BottomRight, TopLeft, BottomLeft;
    public Tilemap Tilemap;
    
    public void AddCollsionTile(Vector2Int pos, int type)
    {
        CollisionTile tile = new CollisionTile(pos, type);
        
        map.Add(tile);
    }

    public void BuildMap()
    {
        foreach(CollisionTile tile in map)
        {
            Vector2Int pos = tile.pos;
            if (pos.x > gridWidthMax) gridWidthMax = pos.x;
            if (pos.y > gridHeightMax) gridHeightMax = pos.y;
            if (pos.x < gridWidthMin) gridWidthMin = pos.x;
            if (pos.y < gridHeightMin) gridHeightMin = pos.y;
        }
        int width = gridWidthMax - gridWidthMin;
        int height = gridHeightMax - gridHeightMin;
        mapGrid = new CollisionTile[width,height];

        print(mapGrid.Length + " " + mapGrid.GetLength(0));
        foreach (CollisionTile tile in map)
        {
            mapGrid[tile.pos.x - gridWidthMin, tile.pos.y - gridHeightMin] = tile;
        }
    }

    public void DebugPlaceTiles()
    {
        BuildMap();
        foreach(CollisionTile tile in map)
        {
            if(tile.type > 0)
            {
                UtilityTilemap.PlaceTile(Tilemap, (Vector3Int)tile.pos, Center);
                Debug.Log("Placing Tile");
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

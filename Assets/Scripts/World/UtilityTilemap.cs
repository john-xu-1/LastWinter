using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class UtilityTilemap
{
    public static void PlaceTile (Tilemap tilemap, Vector3Int pos, TileBase tileType)
    {
        tilemap.SetTile(pos, tileType);
    }

    public static void DestroyTile(Tilemap tilemap, Vector3Int pos)
    {
        tilemap.SetTile(pos, null);
    }

    public static void DestroyTile(Tilemap tilemap, Vector2 pos)
    {
        tilemap.SetTile(tilemap.WorldToCell((Vector3)pos), null);
    }

    public static void PlaceTiles(Tilemap tilemap, Vector3Int pos, float height, float width, TileBase tileType)
    {
        int xStart = pos.x - (int)(width / 2);
        int yStart = pos.y - (int)(height / 2);
        for (int i = xStart; i < xStart + width; i += 1)
        {
            for (int j = yStart; j < yStart + height; j += 1)
            {
                PlaceTile(tilemap, new Vector3Int(i, j, pos.z), tileType);
            }
        }
    }

    public static TileBase GetTile(Tilemap tilemap, Vector2 pos)
    {
        Vector3Int tilePos = tilemap.WorldToCell(pos);
        return tilemap.GetTile(tilePos);
    }


}

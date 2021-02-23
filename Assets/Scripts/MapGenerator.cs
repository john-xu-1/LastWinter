using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public CollisionMap CMap;
    public Transform Centroid;
    public void ConvertMap(string bitmap)
    {
        string[] rows = bitmap.Split('\n');
        int rowCount = 0;
        foreach (string row in rows)
        {
            int colCount = 0;
            foreach (char bit in row)
            {
                CMap.AddCollisionTiles(new Vector2Int(colCount, -rowCount), (int)char.GetNumericValue(bit));
                colCount += 1;
            }
            rowCount += 1;
        }

        CMap.DebugPlaceTiles();
    }

    public void ConvertMap(NewMap newMap)
    {
        foreach (NewTile tile in newMap.area)
        {
            //print(tile.x + ", " + -tile.y + ", " + tile.type);
            CMap.AddCollisionTiles(new Vector2Int(tile.x, -tile.y), tile.type);
        }
        CMap.DebugPlaceTiles();
        Vector2 centroid = CMap.CalculateCentroid();
        print(centroid);
        if (Centroid)Centroid.position = centroid;
    }


}

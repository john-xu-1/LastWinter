using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateBlocks : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase tilebase;
    // Start is called before the first frame update
    void Start()
    {
        UtilityTilemap.PlaceTiles(tilemap, new Vector3Int(0, 0, 0), 11, 1, tilebase);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

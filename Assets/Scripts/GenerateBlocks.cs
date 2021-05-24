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
        print("GenerateBlocks is on " + gameObject.name);
        UtilityTilemap.PlaceTiles(tilemap, new Vector3Int(3, 4, 0), 2, 3, tilebase);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

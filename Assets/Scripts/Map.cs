using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Map 
{
    public Tile[] area;
}

[System.Serializable]
public class Tile
{
    public int x;
    public int y;
    public int type;
}

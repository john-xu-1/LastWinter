using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Map
{
    public Tile[] area;
    public Vector2 start;
    public Dimensions dimensions;
    public Door[] doors;
}

[System.Serializable]
public class Tile
{
    public int x;
    public int y;
    public int type;
}

[System.Serializable]
public class Dimensions
{
    public int room_width;
    public int room_height;
    public int room_count_width;
    public int room_count_height;
}

[System.Serializable]
public class Door
{
    public int source;
    public int destination;
}
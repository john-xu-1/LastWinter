using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    [System.Serializable]
    public class Map
    {
        public Tile[] area;
        public Vector2 start;
        public Dimensions dimensions;
        
        public Path[] paths;
        public PathStart[] pathStarts;
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
    public class Path
    {
        public int x;
        public int y;
        public string type;
    }
    [System.Serializable]
    public class PathStart : Path
    {
        //public int x;
        //public int y;
        //public string type;
    }

    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DebugMap : MonoBehaviour
{
    public MapGenerator Generator;
    public int[][] MyMap;

    [TextArea(5, 10)]
    public string Bitmap;
    //private static string jsonTest = "{\"area\" : {(0,1,1),(0,2,1),(0,0,0)}}";
    //private static string jsonTest = "{\"area\": \"helow\"}";
    private static string jsonTest = "{\"area\": [\"helow\", \"no\"], \"location\" : 1}";



    private void Start()
    {
        //if (Generator) Generator.ConvertMap(Bitmap);
        (int, int, int) tuple = (1, 2, 3);
        print(tuple.Item3);
        NewMap map = JsonUtility.FromJson<NewMap>(jsonTest);
        print(map.area[1]);
    }
}

[System.Serializable]
public class NewMap
{
    //public List<(int, int, int)> area;
    //public string area;
    public string[] area;
    public int location;
}

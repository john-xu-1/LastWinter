using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DebugMap : MonoBehaviour
{
    public MapGenerator Generator;
    public string JsonMapName = "jsonTest.txt";

    [TextArea(5, 10)]
    public string Bitmap;
    private static string jsonTest = "{\"area\" : [{\"x\": 0, \"y\":1,\"type\":1},{\"x\": 1,\"y\": 2,\"type\": 1},{\"x\": 0,\"y\": 1,\"type\": 1}]}";

    private void Start()
    {
        //if (Generator) Generator.ConvertMap(Bitmap);

        StreamReader reader = new StreamReader("Assets/Resources/" + JsonMapName);
        string jsonStr = reader.ReadToEnd();
        //print(jsonStr);
        NewMap map = JsonUtility.FromJson<NewMap>(jsonStr);
        //print(map.area[1].x + ", " + map.area[1].y + ", " + map.area[1].type);
        if (Generator) Generator.ConvertMap(map);
    }


}

[System.Serializable]
public class NewMap
{
    public List<NewTile> area;
}

[System.Serializable]
public class NewTile
{
    public int x;
    public int y;
    public int type;

}
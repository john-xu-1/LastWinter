using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DebugMap : MonoBehaviour
{
    public MapGenerator Generator;
    public int[][] MyMap;

    [TextArea(5, 10)]
    public string Bitmap;

    public string JsonMapName = "test2.txt";

    private void Start()
    {
        //StreamReader reader = new StreamReader("Assets/Resources/" + JsonMapName);
        //string jsonStr = reader.ReadToEnd();
        //Object file = Resources.Load(JsonMapName) as TextAsset;
        TextAsset jsonFile = Resources.Load<TextAsset>(JsonMapName);
        print(jsonFile);
        string jsonStr = jsonFile.text;
        Map map = JsonUtility.FromJson<Map>(jsonStr);
        Generator.ConvertMap(map); 
    }
}

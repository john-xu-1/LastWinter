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
        
        TextAsset jsonFile = Resources.Load<TextAsset>(JsonMapName);
        print(jsonFile);
        string jsonStr = jsonFile.text;
        Map map = JsonUtility.FromJson<Map>(jsonStr);
        Generator.ConvertMap(map); 
    }
}

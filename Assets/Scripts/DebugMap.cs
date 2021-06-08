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

    public string JsonMapName = "test2";

    private void Start()
    {
        TextAsset jfile = Resources.Load<TextAsset>(JsonMapName);
        string jsonstr = jfile.text;
        Map map = JsonUtility.FromJson<Map>(jsonstr);
        Generator.ConvertMap(map); 
    }
}

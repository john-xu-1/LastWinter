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
        StreamReader reader = new StreamReader("Assets/Resources/" + JsonMapName);
        string jsonStr = reader.ReadToEnd();
        Map map = JsonUtility.FromJson<Map>(jsonStr);
        Generator.ConvertMap(map);
        //if (Generator) Generator.ConvertMap(Bitmap); 

 
        //while (line != "")
        //{
        //    print(line);
        //    line = reader.ReadLine();
        //}
        
        
    }
}


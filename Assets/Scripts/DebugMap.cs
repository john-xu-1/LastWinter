using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMap : MonoBehaviour
{
    public MapGenerator Generator;
    public int[][] MyMap;

    [TextArea(5, 10)]
    public string Bitmap;

    private void Start()
    {
        if (Generator) Generator.ConvertMap(Bitmap); 
    }
}

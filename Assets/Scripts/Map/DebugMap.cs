using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMap : MonoBehaviour
{
    public MapGenerator Generator;

    [TextArea(20,30)]
    public string Bitmap;

    private void Start()
    {
        if (Generator) Generator.ConvertMap(Bitmap);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeObjects
{
    public float x, y;
    public enum FreeObjectsTypes
    {
        None,
        Key,
        Enemy,
        Environment
    }
    public FreeObjects type;
    public string objectvariation;

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    [SerializeField] bool debugging;
    [SerializeField] DebugSource[] debugSources;
    public enum DebugTypes
    {
        none,
        tile_rules
    }
    public bool Debug(DebugTypes source)
    {
        if (!debugging) return false;
        else
        {
            foreach(DebugSource debugSource in debugSources)
            {
                if (debugSource.source == source) return debugSource.debug;
            }
            return true;
        }
    }
}
[System.Serializable]
public class DebugSource
{
    public bool debug;
    public Debugger.DebugTypes source;
}

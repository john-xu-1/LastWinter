using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debugging
{
    public class Debugger : MonoBehaviour
    {
        [SerializeField] bool debugging;
        [SerializeField] DebugSource<DebugTypes>[] debugSources;
        [SerializeField] DebugSource<string>[] debugStrSources;
        public enum DebugTypes
        {
            none,
            tile_rules,
            debugData
        }
        public bool Debug(DebugTypes source)
        {
            if (!debugging) return false;
            else
            {
                foreach (DebugSource<DebugTypes> debugSource in debugSources)
                {
                    if (debugSource.source == source) return debugSource.debug;
                }
                return true;
            }
        }
        public bool Debug(string source)
        {
            if (!debugging) return false;
            else
            {
                foreach (DebugSource<string> debugSource in debugStrSources)
                {
                    if (debugSource.source == source) return debugSource.debug;
                }
                return true;
            }
        }
    }
    [System.Serializable]
    public class DebugSource<T>
    {
        public bool debug;
        public T source;
    }
}
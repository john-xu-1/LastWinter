using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public GameObject LoadingScreen;
    public DungeonHandler dungeonHandler;
    
    void Start()
    {
        dungeonHandler.MapSetup(dungeonHandler.worlds.BuiltWorlds[0]);
    }

    
    void Update()
    {
        
    }
}

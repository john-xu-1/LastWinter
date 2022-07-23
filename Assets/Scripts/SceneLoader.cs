using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public DungeonHandler dungeonHandler;

    // Start is called before the first frame update
    void Start()
    {
        dungeonHandler.MapSetup(dungeonHandler.worlds.BuiltWorlds[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

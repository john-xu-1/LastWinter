using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WorldBuilder;

public class LevelSelection : MonoBehaviour
{
    public GameObject levelSelectionPanel;
    public DungeonHandler dh;
    public Worlds worlds;
    List<string> worldNames = new List<string>();
    List<string> worldPaths = new List<string>();
    private Dropdown dpd;
    
    

    void Start()
    {
        levelSelectionPanel.transform.parent.gameObject.SetActive(true);
        dpd = levelSelectionPanel.transform.GetChild(1).GetComponent<Dropdown>();
        

        for (int i = 0; i < WorldBuilder.SaveUtility.getFileNames
            
            
            
            
            
            
            
            ().Length; i++)
        {
            worldPaths.Add(WorldBuilder.SaveUtility.getFileNames()[i]);
            char[] separators = { '/', '\\' };
            string[] texts = WorldBuilder.SaveUtility.getFileNames()[i].Split(separators);
            worldNames.Add(texts[texts.Length - 1]);
        }
        
        dpd.AddOptions(worldNames);
    }

    

    public void genWorld()
    {
        string jsonStr;
        
        if (dpd.value > 0)
        {
            jsonStr = WorldBuilder.SaveUtility.GetFile(worldPaths[dpd.value - 1]);
            Debug.Log(jsonStr);
        }
        else
        {
            jsonStr = "";
        }

        if (jsonStr != "")
        {
            World world = JsonUtility.FromJson<World>(jsonStr);

            dh.MapSetup(world);
            dh.PlayerSetup();
            dh.camSetup();

            levelSelectionPanel.transform.parent.gameObject.SetActive(false);
        }
        
    }
}

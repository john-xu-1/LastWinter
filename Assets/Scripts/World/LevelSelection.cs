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
        

        for (int i = 0; i < WorldBuilder.Utility.getFileNames().Length; i++)
        {
            worldPaths.Add(WorldBuilder.Utility.getFileNames()[i]);
            string[] texts = WorldBuilder.Utility.getFileNames()[i].Split('/');
            worldNames.Add(texts[texts.Length - 1]);
        }
        
        dpd.AddOptions(worldNames);
    }

    

    public void genWorld()
    {
        string jsonStr;
        
        if (dpd.value > 0)
        {
            jsonStr = WorldBuilder.Utility.GetFile(worldPaths[dpd.value - 1]);
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

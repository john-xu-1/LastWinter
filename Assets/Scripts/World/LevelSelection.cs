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
    private World[] maps;
    private Dropdown dpd;
    
    

    void Start()
    {
        maps = worlds.GetWorlds();
        levelSelectionPanel.transform.parent.gameObject.SetActive(true);
        dpd = levelSelectionPanel.transform.GetChild(1).GetComponent<Dropdown>();
        List<string> worldNames = new List<string>();
        for (int i = 0; i < maps.Length; i++)
        {
            worldNames.Add(maps[i].name);
        }

        dpd.AddOptions(worldNames);
    }

    

    public void genWorld()
    {

        World curWorld;
        if (dpd.value > 0)
        {
            curWorld = maps[dpd.value - 1];
        }
        else
        {
            curWorld = null; ;
        }
        dh.MapSetup(curWorld);
        dh.PlayerSetup();
        dh.camSetup();

        levelSelectionPanel.transform.parent.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSaves
{
    public List<string> SaveList = new List<string>();
}

public class PlayerSave
{
    public string saveName;
    public PlayerStats playerStats;
    public bool[] inventory;
    public int[] inventorySelection;

    public PlayerSave(string saveName, PlayerStats ps, bool[] inventory)
    {
        this.saveName = saveName;
        this.playerStats = ps;
        this.inventory = inventory;
    }

    private static void checkSaveList(string saveName)
    {
        //check for "SaveList"
        PlayerSaves SaveList = Utility.LoadFromPlayerPrefs<PlayerSaves>("SaveList");
        if (SaveList != null)
        {
            if (!SaveList.SaveList.Contains(saveName))
            {
                SaveList.SaveList.Add(saveName);
                Utility.SaveJasonToPlayerPrefs<PlayerSaves>(SaveList, "SaveList");
            }
        }
        else
        {
            Debug.Log("It is null");
            SaveList = new PlayerSaves();
            SaveList.SaveList.Add(saveName);
            Utility.SaveJasonToPlayerPrefs<PlayerSaves>(SaveList, "SaveList");
        }
    }

    public static void RemovePlayerSave (string key)
    {
        PlayerSaves SaveList = Utility.LoadFromPlayerPrefs<PlayerSaves>("SaveList");
        if (SaveList != null)
        {
            if (SaveList.SaveList.Contains (key)) SaveList.SaveList.Remove(key);
        }

        PlayerSave playerSave = Utility.LoadFromPlayerPrefs<PlayerSave>(key);
        if (playerSave != null) PlayerPrefs.DeleteKey(key);
    }

    public static void SavePlayerStats(string saveName, PlayerStats playerStats)
    {
        checkSaveList(saveName);


        PlayerSave playerSave = Utility.LoadFromPlayerPrefs<PlayerSave>(saveName);
        if (playerSave == null)
        {
            playerSave = new PlayerSave(saveName, playerStats, new bool[0]);
        }

        playerSave.playerStats = playerStats;

        //save PlayerSave
        Utility.SaveJasonToPlayerPrefs<PlayerSave>(playerSave, saveName);
    }

    public static void SaveInventory(string saveName, bool[] inventory, int[] inventorySelection)
    {
        checkSaveList(saveName);
        //check for saveName & load PlayerSave
        PlayerSave playerSave = Utility.LoadFromPlayerPrefs<PlayerSave>(saveName);
        if (playerSave == null)
        {
            playerSave = new PlayerSave(saveName, new PlayerStats(), inventory);
        }
        //update PlayerSave

        playerSave.inventory = inventory;
        playerSave.inventorySelection = inventorySelection;

        //save PlayerSave
        Utility.SaveJasonToPlayerPrefs<PlayerSave>(playerSave, saveName);
    }

    public static bool[] GetInventory(string saveName)
    {
        PlayerSave playerSave = Utility.LoadFromPlayerPrefs<PlayerSave>(saveName);
        if (playerSave != null) return playerSave.inventory;
        else return null;
    }

    public static int[] GetInventorySelection(string saveName)
    {
        PlayerSave playerSave = Utility.LoadFromPlayerPrefs<PlayerSave>(saveName);
        if (playerSave != null) return playerSave.inventorySelection;
        else return null;
    }
}


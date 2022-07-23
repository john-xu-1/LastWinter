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
    public int equipedItem = -1;

    public PlayerSave(string saveName, PlayerStats ps, bool[] inventory, int equipedItem)
    {
        this.saveName = saveName;
        this.playerStats = ps;
        this.inventory = inventory;
        this.equipedItem = equipedItem;
    }

    //public static PlayerSave GetPlayerSave(string saveName)
    //{

    //}

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

    public static string[] GetPlayerSaveList()
    {
        PlayerSaves SaveList = Utility.LoadFromPlayerPrefs<PlayerSaves>("SaveList");


        if (SaveList != null)
        {
            string[] saveList = new string[SaveList.SaveList.Count];

            for (int i = 0; i < SaveList.SaveList.Count; i += 1)
            {
                saveList[i] = SaveList.SaveList[i];
                
            }

            return saveList;
        }
        else
        {
            return new string[0];
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
            playerSave = new PlayerSave(saveName, playerStats, new bool[0], playerSave.equipedItem);
        }

        playerSave.playerStats = playerStats;

        //save PlayerSave
        Utility.SaveJasonToPlayerPrefs<PlayerSave>(playerSave, saveName);
    }

    public static void SaveInventory(string saveName, bool[] inventory, int[] inventorySelection, int equipedItem)
    {
        checkSaveList(saveName);
        //check for saveName & load PlayerSave
        PlayerSave playerSave = Utility.LoadFromPlayerPrefs<PlayerSave>(saveName);
        if (playerSave == null)
        {
            playerSave = new PlayerSave(saveName, new PlayerStats(), inventory, equipedItem);
        }
        //update PlayerSave

        playerSave.inventory = inventory;
        playerSave.inventorySelection = inventorySelection;
        playerSave.equipedItem = equipedItem;

        //save PlayerSave
        Utility.SaveJasonToPlayerPrefs<PlayerSave>(playerSave, saveName);
    }

    public static bool[] GetInventory(string saveName)
    {
        PlayerSave playerSave = Utility.LoadFromPlayerPrefs<PlayerSave>(saveName);
        if (playerSave != null) return playerSave.inventory;
        else return null;
    }

    public static int[] GetInventorySelection (string saveName)
    {
        PlayerSave playerSave = Utility.LoadFromPlayerPrefs<PlayerSave>(saveName);
        if (playerSave != null) return playerSave.inventorySelection;
        else return null;
    }

    public static int GetEquipedItem(string saveName)
    {
        PlayerSave playerSave = Utility.LoadFromPlayerPrefs<PlayerSave>(saveName);
        if (playerSave != null) return playerSave.equipedItem;
        else return -1;
    }
}


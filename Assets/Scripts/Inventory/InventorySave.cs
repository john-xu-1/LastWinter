using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventorySave", menuName = "CustomObject/Inventory")]
public class InventorySave : ScriptableObject
{
    [SerializeField] private List<InventoryObjects> inventoryObjects;
    [SerializeField] private InventoryObjects[] allInventoryObjects;

    public List<InventoryObjects> GetInventory()
    {
        return inventoryObjects;
    }

    public void SetInventory(List<InventoryObjects> inventoryObjects)
    {
        this.inventoryObjects = inventoryObjects;
    }

    public void SaveInventory(string saveName)
    {
        bool[] inventory = new bool[allInventoryObjects.Length];
        for (int i = 0; i < allInventoryObjects.Length; i += 1)
        {
            if (inventoryObjects.Contains(allInventoryObjects[i]))
            {
                inventory[i] = true;
            }
        }
        PlayerSave.SaveInventory(saveName, inventory);
    }

    public void LoadInventory(string saveName)
    {

    }
}

public class PlayerSaves
{
    public string[] SaveList;
}

public class PlayerSave
{
    public string saveName;
    public PlayerStats playerStats;
    public bool[] inventory;

    public PlayerSave (string saveName, PlayerStats ps, bool[] inventory)
    {
        this.saveName = saveName;
        this.playerStats = ps;
        this.inventory = inventory;
    }

    public static void SaveInventory(string saveName, bool[] inventory)
    {
        //check for "SaveList"
        PlayerSaves SaveList = Utility.LoadFromPlayerPrefs<PlayerSaves>("SaveList");
        if (SaveList != null)
        {

        }
        else
        {
            Debug.Log("It is null");
        }
        //check for saveName
        //load PlayerSave
        //update PlayerSave
        //save PlayerSave
    }
}

public class PlayerStats
{
    public int HP;
    public Vector2 Pos;
}



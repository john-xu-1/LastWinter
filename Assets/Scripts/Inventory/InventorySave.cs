using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventorySave", menuName = "CustomObject/Inventory")]
public class InventorySave : ScriptableObject
{
    [SerializeField] private List<InventoryObjects> inventoryObjects;
    [SerializeField] private InventoryObjects[] allInventoryObjects;
    [SerializeField] private string saveName;

    public List<InventoryObjects> GetInventory()
    {
        LoadInventory(saveName);
        return inventoryObjects;
    }

    public void SetInventory(List<InventoryObjects> inventoryObjects)
    {
        this.inventoryObjects = inventoryObjects;
    }

    public void SaveInventory(string saveName)
    {
        this.saveName = saveName;
        SaveInventory();
    }

    public void SaveInventory()
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
        inventoryObjects.Clear();
        bool[] inventory = PlayerSave.GetInventory(saveName);
        if (inventory != null)
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i])
                {
                    inventoryObjects.Add(allInventoryObjects[i]);
                }
            }
        }
    }
}




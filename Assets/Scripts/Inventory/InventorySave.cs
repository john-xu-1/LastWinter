using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventorySave", menuName = "CustomObject/Inventory")]
public class InventorySave : ScriptableObject
{
    [SerializeField] private List<InventoryObjects> inventoryObjects;
    [SerializeField] private InventoryObjects[] allInventoryObjects;
    [SerializeField] private string saveName;
    public int currentItem = -1;

    public void SaveName (string saveName)
    {
        this.saveName = saveName;
    }
    

    public List<InventoryObjects> GetInventory()
    {
        LoadInventory(saveName);
        return inventoryObjects;
    }

    public void SetInventory(List<InventoryObjects> inventoryObjects, int currentItem)
    {
        this.inventoryObjects = inventoryObjects;
        this.currentItem = currentItem;
    }

    public void SaveInventory(string saveName)
    {
        this.saveName = saveName;
        SaveInventory();
    }

    public void SaveInventory()
    {
        int[] inventorySelection = new int[inventoryObjects.Count];
        for (int i = 0; i < inventorySelection.Length; i += 1)
        {
            inventorySelection[i] = GetItemID(inventoryObjects[i]);
        }


        bool[] inventory = new bool[allInventoryObjects.Length];
       
        
        for (int i = 0; i < allInventoryObjects.Length; i += 1)
        {
            if (inventoryObjects.Contains(allInventoryObjects[i]))
            {
                inventory[i] = true;
            }
        }
        PlayerSave.SaveInventory(saveName, inventory, inventorySelection, currentItem);
    }

    private int GetItemID(InventoryObjects item)
    {
        for (int i = 0; i < allInventoryObjects.Length; i += 1)
        {
            if (item == allInventoryObjects[i])
            {
                return i;
            }
        }

        return -1;
    }

    public void LoadInventory(string saveName)
    {
        inventoryObjects.Clear();
        bool[] inventory = PlayerSave.GetInventory(saveName);
        int[] inventorySelection = PlayerSave.GetInventorySelection(saveName);
        currentItem = PlayerSave.GetEquipedItem(saveName);

        if (inventorySelection != null)
        {
            for (int i = 0; i < inventorySelection.Length; i++)
            {
                inventoryObjects.Add(allInventoryObjects[inventorySelection[i]]);
            }
        }

        
    }
}




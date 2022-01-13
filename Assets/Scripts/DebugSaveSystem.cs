using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSaveSystem : MonoBehaviour
{
    public InventorySave IS;
    public void SaveInventoryButton()
    {
        IS.SaveInventory("Player 1");
    }

    public void RemoveSaveButton()
    {
        PlayerSave.RemovePlayerSave("Player 1");
    }
}

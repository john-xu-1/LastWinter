using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSaveSystem : MonoBehaviour
{
    public InventorySave IS;
    public string saveName = "Player 2";
    public void LoadLabButon()
    {
        IS.SetSaveName(saveName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    //public void SaveInventoryButton()
    //{
    //    IS.SaveInventory("Player 1");
    //}

    public void DeleteSaveButton()
    {
        PlayerSave.RemovePlayerSave(saveName);
    }
}

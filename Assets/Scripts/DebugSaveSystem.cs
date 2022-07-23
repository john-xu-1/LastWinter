using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugSaveSystem : MonoBehaviour
{
    public InventorySave IS;
    public string saveName = "Player 2";
    

    public void LoadLab()
    {
        IS.SaveName(saveName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    //public void SaveInventoryButton()
    //{
    //    IS.SaveInventory("Player 1");
    //}

    public void DeleteSaveButton()
    {
        PlayerSave.RemovePlayerSave (saveName);
    }

    //public void setSaveName(string s)
    //{

    //    saveName = s;
    //}
}

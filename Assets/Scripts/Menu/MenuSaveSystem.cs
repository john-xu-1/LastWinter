using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSaveSystem : MonoBehaviour
{
    public InventorySave IS;
    [SerializeField] private string saveName = "";

    public MenuUIHandler MUH;

    public int labIndex = 1;


    public void LoadLab()
    {
        IS.SaveName(saveName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(labIndex);
    }

    public void DeleteSaveButton()
    {
        PlayerSave.RemovePlayerSave(saveName);
        MUH.removeContent(saveName);
    }

    public void setName(string s)
    {
        saveName = s;
    }

    

}

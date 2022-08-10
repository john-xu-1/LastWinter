using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUIHandler : MonoBehaviour
{
    private List<string> array = new List<string>();

    public float InstantiateInterval;

    public Vector2 currentPoint;

    public GameObject PlayerShowcasePrefab;

    public InventorySave ISave;

    public MenuSaveSystem mss;

    private List<GameObject> allContent = new List<GameObject>();

    private void Awake()
    {
        string[] placeholder = PlayerSave.GetPlayerSaveList();
        foreach (string item in placeholder)
        {
            array.Add(item);
        }

        for (int i = 0; i < array.Count; i+=1)
        {
            if (i != 0) currentPoint = new Vector2(currentPoint.x + InstantiateInterval, currentPoint.y);


            GameObject instance = Instantiate(PlayerShowcasePrefab, currentPoint, Quaternion.identity);

            instance.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = array[i];

            ISave.SaveName(array[i]);

            //Debug.Log(ISave.GetSaveName());

            List<InventoryObjects> theList = ISave.GetInventory();

            //Debug.Log(ISave.GetInventory());
            

            List<string> playerItemNames = new List<string>();
            List<Sprite> playerItemImage = new List<Sprite>();

            GameObject cont = null;

            if (theList != null && theList.Count > 1)
            {
                for (int j = 0; j < theList.Count; j += 1)
                {
                    playerItemNames.Add(theList[j].itemName);
                    playerItemImage.Add(theList[j].itemSprite);
                }

                cont = instance.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0).gameObject;

                cont.transform.GetComponent<MainMenuSelectionInventory>().PopulateContent(playerItemImage, playerItemNames);
                
            }

            allContent.Add(cont);


        }
    }

    public void removeContent (string name)
    {
        int ind = array.IndexOf(name);

        int size = 0;

        if (allContent[ind].transform.GetChild(0).gameObject) size = allContent[ind].transform.childCount;

        if (size > 0)
        {
            for (int i = 0; i < size; i += 1)
            {
                Destroy(allContent[ind].transform.GetChild(i).gameObject);
            }
        }

        
    }

    public void setSaveName(int i)
    {
        mss.setName(array[i]);
    }

    public void returnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    

    public List<string> getArray()
    {
        return array;
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject[] InventoryPictures;

    public GameObject[] InventoryFrames;

    public Image selectedSlot;

    public Sprite selframeSprite;
    public Sprite unselframeSprite;

    public void updateSprite(List<InventoryObjects> items, InventoryObjects selectedSprite)
    {
        

        for (int i = 0; i < items.Count; i += 1)
        {
            if (items[i] != null)
            {
                InventoryPictures[i].GetComponent<Image>().sprite = items[i].itemSprite;
                InventoryPictures[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);


            }
            else
            {
                InventoryPictures[i].GetComponent<Image>().sprite = null;
                InventoryPictures[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }

        }
    }

    public void updateSelectedUI (List<InventoryObjects> items, InventoryObjects selectedSprite)
    {
        if (selectedSprite != null)
        {

            for (int i = 0; i < items.Count; i += 1)
            {
                InventoryFrames[i].GetComponent<Image>().sprite = unselframeSprite;
            }

            selectedSlot.GetComponent<Image>().sprite = selectedSprite.itemSprite;
            selectedSlot.GetComponent<Image>().color = new Color(1, 1, 1, 1);

            InventoryFrames[items.IndexOf(selectedSprite)].GetComponent<Image>().sprite = selframeSprite;

            
        }
        else
        {
            for (int i = 0; i < items.Count; i += 1)
            {
                InventoryFrames[i].GetComponent<Image>().sprite = unselframeSprite;
            }

            Debug.Log("esuh");

            selectedSlot.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        }

        
    }

    public void updateSprite(int maxSize, InventoryObjects[] items, InventoryObjects selectedSprite)
    {

        for (int i = 0; i < maxSize; i += 1)
        {
            if (items[i] != null)
            {
                InventoryPictures[i].GetComponent<Image>().sprite = items[i].itemSprite;
                InventoryPictures[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);

            }
            else
            {
                InventoryPictures[i].GetComponent<Image>().sprite = null;
                InventoryPictures[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }

        }

    }

    public void setDropdown(Dropdown dpd, List<string> list)
    {
        dpd.AddOptions(list);
    }

}

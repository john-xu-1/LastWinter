using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject[] InventoryPictures;

    public void updateSprite(List<InventoryObjects> items)
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

    public void updateSprite(int maxSize, InventoryObjects[] items)
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

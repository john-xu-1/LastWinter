using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject[] InventoryPictures;

    public void updateSprite(int maxSize, List<InventoryObjects> items)
    {
        for (int i = 0; i < maxSize; i += 1)
        {
            if (items[i] != null)
            {
                InventoryPictures[i].GetComponent<Image>().sprite = items[i].itemSprite;
                
            }
            else
            {
                InventoryPictures[i].GetComponent<Image>().sprite = null;
            }

        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject[] InventoryPictures;

    private void Update()
    {
        for (int i = 0; i < FindObjectOfType<InventorySystem>().items.Length; i += 1)
        {
            if (FindObjectOfType<InventorySystem>().items[i])
            {
                InventoryPictures[i].GetComponent<Image>().sprite = FindObjectOfType<InventorySystem>().items[i].itemSprite;
            }
            
        }

        
    }


}

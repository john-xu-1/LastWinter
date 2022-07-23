using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSelectionInventory : MonoBehaviour
{
    public GameObject InventorySlotPrefab;

    public float spacing = 0.75f;

    public float offset = 2f;

    public void PopulateContent(List<Sprite> images, List<string> names)
    {

        float y = 0;
        y -= offset;

        //Debug.Log("instante");

        for (int i = 0; i < images.Count; i += 1)
        {

            GameObject instance = Instantiate(InventorySlotPrefab, transform);

            //Debug.Log("instante");

            y -= spacing;

            instance.transform.position = new Vector3(instance.transform.position.x, y);

            instance.transform.GetChild(0).GetComponent<Image>().sprite = images[i];
            instance.transform.GetChild(1).GetComponent<Text>().text = names[i];
        }

        
    }

    public void deleteChild(int index)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class InventoryChest : MonoBehaviour
{
    public Color chestColor;

    public Color[] colorLibrary = new Color[4];

    public string rarity;

    public List<InventoryObjects> content;

    public GameObject emptyPrefab;

    public float objectEjectSpeed;

    public Vector2 minMaxRandX;

    public float interactRange = 2;

    

    private void Start()
    {
        if (rarity == "S")
        {
            chestColor = colorLibrary[0];
        }
        else if (rarity == "A")
        {
            chestColor = colorLibrary[1];
        }
        else if (rarity == "B")
        {
            chestColor = colorLibrary[2];
        }
        else if (rarity == "C")
        {
            chestColor = colorLibrary[3];
        }

        transform.GetChild(1).GetComponent<SpriteRenderer>().color = chestColor;

        
        transform.GetChild(2).GetComponent<Light2D>().color = chestColor;
    }

    public void addItems(InventoryObjects[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            content.Add(items[i]);
        }
    }

    public void addItems(List<InventoryObjects> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            content.Add(items[i]);
        }
    }


    public void onInteract()
    {
        for (int i = 0; i < content.Count; i++)
        {

            GameObject instance = Instantiate(emptyPrefab, transform.position, Quaternion.identity);

            instance.GetComponent<Pickupable>().item = content[i];

            instance.GetComponent<SpriteRenderer>().sprite = content[i].itemSprite;

            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();

            float randX = Random.Range(minMaxRandX.x, minMaxRandX.y);

            rb.AddForce(new Vector2(randX, objectEjectSpeed), ForceMode2D.Impulse);

            Debug.Log("doing" + i + " times");
        }

        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Vector2.Distance (transform.position, FindObjectOfType<PlatformerController>().transform.position) <= interactRange)
            {
                onInteract();
            }
        }
    }

}

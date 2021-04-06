using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public InventoryObjects item;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = item.itemSprite;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (FindObjectOfType<InventorySystem>().index <= FindObjectOfType<InventorySystem>().items.Length - 1)
        {
            if (collision.transform.CompareTag("Player"))
            {
                FindObjectOfType<InventorySystem>().AddItem(item);
                Destroy(gameObject);
            }
        }
        
    }
}

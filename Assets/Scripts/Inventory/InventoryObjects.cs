using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItems", menuName = "CustomObject/InventoryItems")]
public class InventoryObjects : ScriptableObject
{
    public enum ItemTypes
    {
        Chip,
        Weapon,
        Accesory
    }
    public ItemTypes itemType;
    public Sprite itemSprite;
    public string itemName;
    public string itemRare;
}

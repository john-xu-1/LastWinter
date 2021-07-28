using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryChip", menuName = "CustomObject/InventoryItems/InvetoryChip")]
public class InventoryChip : InventoryObjects
{
    public enum ChipTypes
    {
        fire,
        water,
        astro,
        lightning,
        mirror,
        leaf
    }

    public ChipTypes chipType;

    public GameObject trail;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryAccessory", menuName = "CustomObject/InventoryItems/InvetoryAccessory")]
public class InventoryAccessory : InventoryObjects
{
    public enum AcsTypes
    {
        Steam_Pulse,
        Winning_Cup,
        Heart_Flower,
        Precision_Scope,
        Seashell_Necklace
    }
    public AcsTypes acsType;

    public GameObject spawnPrefab;
}

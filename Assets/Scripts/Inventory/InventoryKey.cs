using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryKey", menuName = "CustomObject/InventoryItems/InventoryKey")]
public class InventoryKey : InventoryObjects
{
    public WorldBuilder.GateTypes KeyType;
}

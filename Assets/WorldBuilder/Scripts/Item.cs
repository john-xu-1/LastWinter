using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public Vector2 startPos;
    public WorldBuilder.Items.ItemTypes itemType;
    public string variation;
    //public enum ItemTypes
    //{
    //    None,

    //}
    public abstract void ItemSetup();
    public abstract void Remove();
}

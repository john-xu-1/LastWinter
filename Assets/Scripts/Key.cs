using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{

    public WorldBuilder.GateTypes keyType;
    public SpriteRenderer sprite;
    public override void ItemSetup()
    {
        transform.position = new Vector3(startPos.x, startPos.y, transform.position.z);
        
    }

    public override void Remove()
    {
        Destroy(gameObject);
    }

    public void SetupKey(float x, float y, WorldBuilder.GateTypes keyType)
    {
        
        this.keyType = keyType;
        SetupKeyType(keyType);
        startPos = new Vector2(x, y);
        itemType = WorldBuilder.Items.ItemTypes.key;
        variation = keyType.ToString();
        ItemSetup();
    }
    public void SetupKey(float x, float y, WorldBuilder.GateTypes keyType, Color color)
    {

        SetupKey(x, y, keyType);
        sprite.color = color;
        
    }

    void SetupKeyType(WorldBuilder.GateTypes keyType)
    {
        //to be overwritten change sprites or key behavior
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) gameObject.SetActive(false);
    }
}

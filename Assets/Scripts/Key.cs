using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    public WorldBuilder.GateTypes keyType;
    public SpriteRenderer sprite;
    public void SetupKey(float x, float y, WorldBuilder.GateTypes keyType)
    {
        transform.position = new Vector3(x, y, transform.position.z);
        this.keyType = keyType;
        SetupKeyType(keyType);
    }
    public void SetupKey(float x, float y, WorldBuilder.GateTypes keyType, Color color)
    {
        transform.position = new Vector3(x, y, transform.position.z);
        this.keyType = keyType;
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

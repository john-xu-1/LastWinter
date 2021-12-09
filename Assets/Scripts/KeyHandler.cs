using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHandler : MonoBehaviour
{
    public WorldBuilder.GateTypes KeyType;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (KeyType == WorldBuilder.GateTypes.door)
            {
                FindObjectOfType<DoorHandler>().hasDoorKey = true;
            }
            else if (KeyType == WorldBuilder.GateTypes.enemy)
            {
                FindObjectOfType<DoorHandler>().hasEnemyKey = true;
            }
            else if (KeyType == WorldBuilder.GateTypes.lava)
            {
                //FindObjectOfType<DoorHandler>().hasLavaKey = true;
            }
            else if (KeyType == WorldBuilder.GateTypes.water)
            {
                //FindObjectOfType<DoorHandler>().hasWaterKey = true;
            }
            Destroy(gameObject);
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorHandler : MonoBehaviour
{
    public bool hasDoorKey, hasWaterKey, hasLavaKey, hasEnemyKey;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Vector2 normal = collision.GetContact(0).normal;
            Debug.Log(UtilityTilemap.GetTile(GetComponent<Tilemap>(), (Vector2)collision.transform.position + normal) + " " + normal);
            
            if(normal.y > 0)
            {
                RemoveDoor((Vector2)collision.transform.position + normal + Vector2.up);
            }
            else
            {
                RemoveDoor((Vector2)collision.transform.position + normal);
            }
            
        }
    }

    private void RemoveDoor(Vector2 pos)
    {
        TileBase tile = UtilityTilemap.GetTile(GetComponent<Tilemap>(), pos);
        Debug.Log(tile + " : " + pos);
        if (tile && (tile == FindObjectOfType<CollisionMap>().door && hasDoorKey) || (tile == FindObjectOfType<CollisionMap>().enemyDoor && hasEnemyKey))
        {

            Debug.Log("Destroy Tilebase: " + tile);
            UtilityTilemap.DestroyTile(GetComponent<Tilemap>(), pos);

            RemoveDoor(pos + Vector2.up);
            RemoveDoor(pos - Vector2.up);
            RemoveDoor(pos + Vector2.right);
            RemoveDoor(pos - Vector2.right);
        }
    }
}

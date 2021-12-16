using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class doorHandler : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag ("Player"))
        {
            Debug.Log(UtilityTilemap.GetTile(GetComponent<Tilemap>(), collision.transform.position));
        }
    }
}

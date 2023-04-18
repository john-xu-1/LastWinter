using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public int teleporterID;

    float pauseTriggerUntil = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && pauseTriggerUntil < Time.time)
        {
            foreach(Teleporter teleporter in FindObjectsOfType<Teleporter>())
            {
                if (teleporter != this && teleporter.teleporterID == teleporterID)
                    teleporter.TeleportTo(collision.transform);
            }
        }
    }

    public void TeleportTo(Transform player)
    {
        pauseTriggerUntil = Time.time + 1;
        player.position = transform.position;
    }
}

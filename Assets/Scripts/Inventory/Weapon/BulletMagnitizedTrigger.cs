using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMagnitizedTrigger : MonoBehaviour
{
    public BulletMagnetizedShifter parent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        parent.triggerEnterBehavior(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        parent.triggerExitBehavior(collision);
    }

}

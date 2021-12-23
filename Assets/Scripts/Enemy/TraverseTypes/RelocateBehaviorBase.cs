using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelocateBehaviorBase : MonoBehaviour
{
    public float coolDown = 2f;
    [SerializeField] float nextRelTime;
    public Vector2 Use ()
    {
        if (Time.time >= nextRelTime)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            float x = Random.Range(player.transform.position.x - range(), player.transform.position.x + range());
            float y = Random.Range(player.transform.position.y - range(), player.transform.position.y + range());
            teleport(new Vector2(x, y));
            nextRelTime = Time.time + coolDown;
        }

        return Vector2.zero;

    }

    private void teleport(Vector2 destination)
    {
        transform.position = destination;
    }

    private float range()
    {
        return 10;
    }
}

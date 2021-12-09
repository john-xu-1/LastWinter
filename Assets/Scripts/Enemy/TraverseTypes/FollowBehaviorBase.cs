using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehaviorBase : MonoBehaviour
{
    [SerializeField] float xDistanceBuffer = 0.5f;
    public Vector2 Use(ref Vector2 direction, ref Vector2 velocity)
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        direction = ((player.position - transform.position) * Vector2.right).normalized;

        if (Mathf.Abs(player.position.x - transform.position.x) < xDistanceBuffer) direction.x = 0;
        return new Vector2(direction.x * followSpeed(), velocity.y);
    }

    private float followSpeed()
    {
        return 10;
    }
}

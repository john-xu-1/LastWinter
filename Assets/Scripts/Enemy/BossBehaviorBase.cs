using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviorBase : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;

    public enum AttackTypes
    {
        none,
        melee,
        range,
        area
    }

    public enum TraversingTypes
    {
        none,
        following,
        randomMoving,
        avoiding,
        relocate
    }

    [SerializeField] protected TraversingTypes traversingType;

    private void FixedUpdate()
    {
        handleMotion();
    }
    private void handleMotion()
    {
        rb.velocity = motion();
    }

    private Vector2 motion()
    {
        Vector2 movement = rb.velocity;
        if(traversingType == TraversingTypes.following)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            Vector3 target = player.transform.position;
            Vector2 direction = ((target - transform.position) * Vector2.right).normalized;
            movement = new Vector2( followSpeed() * direction.x, rb.velocity.y);
        }

        return movement;
    }

    private float followSpeed()
    {

        return 5;
    }
}

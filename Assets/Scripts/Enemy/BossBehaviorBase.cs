using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviorBase : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;

    public enum AttackTypes
    {
        none,
        melee,
        range,
        areea
    }

    public enum TraversingTypes
    {
        none,
        following,
        randomMoving,
        aoviding,
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
        if (traversingType == TraversingTypes.following)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            Vector2 direction = ((player.position - transform.position) * Vector2.right).normalized;
            movement = new Vector2(direction.x * followSpeed(), rb.velocity.y);

        }

        return movement;

    }

    private float followSpeed()
    {
        return 10;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviorBase : MonoBehaviour
{
    [SerializeField] protected LayerMask mask;

    [SerializeField] protected Rigidbody2D rb;

    [SerializeField] protected float jumpSpeed = 5;

    private Vector2 direction;

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
        Vector2 movement = motion();

        float wallHeight = getWallHeight(direction.x);

       if (wallHeight > 0 && wallHeight < 4)
        {
            if (isGrounded()) movement = new Vector2(0, wallHeight * jumpSpeed);
            else movement = new Vector2(0, movement.y);
        }

        rb.velocity = movement;
    }

    private float getWallHeight(float dir)
    {
        float rayDistance = 2;
        for (int i = 3; i >= 0; i -= 1)
        {
            float hitDistance = PhysicsObject.raycast(transform.position + Vector3.up * (0.5f + i), dir * Vector3.right, rayDistance, mask);
            if (hitDistance < rayDistance) return i + 1;
        }
        

        return 0;
    }

    private bool isGrounded()
    {
        for (int i = 0; i < 3; i += 1)
        {
            float GroundDistance = PhysicsObject.raycast(transform.position + (-1 * i) *Vector3.right, Vector3.down, 0.1f, mask);
            if (GroundDistance <= 0.1f) return true;
        }
        
        
        return false;
    }

    private Vector2 motion()
    {
        Vector2 movement = rb.velocity;
        if (traversingType == TraversingTypes.following)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            direction = ((player.position - transform.position) * Vector2.right).normalized;
            movement = new Vector2(direction.x * followSpeed(), rb.velocity.y);

        }

        return movement;

    }

    private float followSpeed()
    {
        return 10;
    }

}

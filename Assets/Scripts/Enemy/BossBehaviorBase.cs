using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviorBase : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected LayerMask layerMask;

    [SerializeField] protected float jumpSpeed = 5;

    private Vector2 direction;

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
        Vector2 movement = motion();
        
        float wallHeight = getWallHeight(direction.x);
        Debug.Log(direction + " " + wallHeight);
        if (wallHeight > 0 && wallHeight < 4)
        {
            if (isGrounded()) movement = new Vector2(0, wallHeight * jumpSpeed);
            else movement = new Vector2(0,movement.y);

        }

        rb.velocity = movement;
    }

    private bool isGrounded()
    {
        for(int i = 0; i < 3; i += 1)
        {
            float groundDistance = PhysicsObject.raycast(transform.position + (-1 + i)*Vector3.right, Vector3.down, 0.1f, layerMask);
            if (groundDistance <= 0.1f) return true;
        }
        
        return false;
    }

    private float getWallHeight(float direction)
    {
        float rayDistance = 1.1f;
        for(int i = 3; i >= 0; i -= 1)
        {
            float hitDistance = PhysicsObject.raycast(transform.position + Vector3.up * (0.0f + i), direction * Vector3.right, rayDistance, layerMask);
            
            if (hitDistance <= rayDistance) return i + 1;
        }
        

        return 0;
    }

    private Vector2 motion()
    {
        Vector2 movement = rb.velocity;
        if(traversingType == TraversingTypes.following)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            Vector3 target = player.transform.position;
            direction = ((target - transform.position) * Vector2.right).normalized;
            movement = new Vector2( followSpeed() * direction.x, rb.velocity.y);
        }

        return movement;
    }

    private float followSpeed()
    {

        return 5;
    }
}

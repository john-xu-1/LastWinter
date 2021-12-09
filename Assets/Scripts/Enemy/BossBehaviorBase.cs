using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviorBase : PhysicsObject
{
    [SerializeField] protected LayerMask mask;

    //[SerializeField] protected Rigidbody2D rb;

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

    //private void FixedUpdate()
    //{
    //    handleMotion();
    //}

    protected override void ComputeVelocity()
    {
        targetVelocity = handleMotion();


        velocity.y = targetVelocity.y;


    }

    private Vector2 handleMotion()
    {
        Vector2 movement = motion();

        float wallHeight = getWallHeight(direction.x);

        if (wallHeight > 0 && wallHeight < 4)
        {
            if (isGround())
            {
                movement = new Vector2(0, Mathf.Sqrt(wallHeight) * jumpSpeed);

            }
            else movement = new Vector2(0, movement.y);
        }

        //rb.velocity = movement;
        return movement;
    }

    private float getWallHeight(float dir)
    {
        float rayDistance = 2;
        for (int i = 4; i >= 0; i -= 1)
        {
            float hitDistance = PhysicsObject.raycast(transform.position + Vector3.up * (0.01f + i), dir * Vector3.right, rayDistance, mask);
            if (hitDistance < rayDistance) return i + 1;
        }


        return 0;
    }

    private bool isGround()
    {
        for (int i = 0; i < 3; i += 1)
        {
            float GroundDistance = PhysicsObject.raycast(transform.position + (-1 + i) * Vector3.right, Vector3.down, 0.02f, mask);
            if (GroundDistance <= 0.1f) return true;
        }


        return false;
    }

    private Vector2 motion()
    {
        Vector2 movement = velocity;
        switch (traversingType)
        {
            case TraversingTypes.following:

                movement = GetComponent<FollowBehaviorBase>().Use(ref direction, ref velocity);

                break;
            case TraversingTypes.randomMoving:
                movement = GetComponent<RandomMovingBehaviorBase>().Use(ref direction, ref velocity);
                break;
            case TraversingTypes.avoiding:

                break;
            case TraversingTypes.relocate:

                break;
            default:

                break;
        }


        return movement;

    }



}


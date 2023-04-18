using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorRollaBoi : EnemyBehaviorBase
{
    public bool movingRight = true;
    public LayerMask groundLayer;
    public float radius = 0.5f;

    [SerializeField] private Vector2 gravity = Vector2.down;


    public float startTimeBetweenSpawn;
    private float timeSpawnGap;
    public GameObject trail;
    public float trailTime;


    //called from FixedUpdate
    public override void defaultAI()
    {
        Debug.DrawRay(Vector3.up * radius + transform.position, Vector2.up * speed * Time.fixedDeltaTime, Color.red);
        Debug.DrawRay(Vector3.right * radius + transform.position, Vector2.right * speed * Time.fixedDeltaTime, Color.red);
        Debug.DrawRay(Vector3.down * radius + transform.position, Vector2.down * speed * Time.fixedDeltaTime, Color.red);
        Debug.DrawRay(Vector3.left * radius + transform.position, Vector2.left * speed * Time.fixedDeltaTime, Color.red);

        RaycastHit2D hitUp = HandleRaycast(Vector2.up, speed * Time.fixedDeltaTime);
        RaycastHit2D hitRight = HandleRaycast(Vector2.right, speed * Time.fixedDeltaTime);
        RaycastHit2D hitDown = HandleRaycast(Vector2.down, speed * Time.fixedDeltaTime);
        RaycastHit2D hitLeft = HandleRaycast(Vector2.left, speed * Time.fixedDeltaTime);

        if (gravity.y < 0) //down
        {
            if (movingRight) ///movingRight
            {
                HandleMovement(hitRight, hitDown, hitLeft, Vector2.right, Vector2.down, Vector2.left);

            }
            else HandleMovement(hitLeft, hitDown, hitRight, Vector2.left, Vector2.down, Vector2.right);
            
        }
        else if (gravity.y > 0)//up
        {
            if (movingRight) //movingLeft
            {
                HandleMovement(hitLeft, hitUp, hitRight, Vector2.left, Vector2.up, Vector2.right);

            } else HandleMovement(hitRight, hitUp, hitLeft, Vector2.right, Vector2.up, Vector2.left);

        }
        else if (gravity.x < 0)//left
        {
            if (movingRight) //movingDown
            {
                HandleMovement(hitDown, hitLeft, hitUp, Vector2.down, Vector2.left, Vector2.up);

            } else HandleMovement(hitUp, hitLeft, hitDown, Vector2.up, Vector2.left, Vector2.down);
        }
        else if (gravity.x > 0)//right
        {
            if (movingRight) //movingUp
            {
                HandleMovement(hitUp, hitRight, hitDown, Vector2.up, Vector2.right, Vector2.down);

            } else HandleMovement(hitDown, hitRight, hitUp, Vector2.down, Vector2.right, Vector2.up);

        }




        if (timeSpawnGap <= 0)
        {
            GameObject instance = Instantiate(trail, transform.position, transform.rotation);

            Destroy(instance, trailTime);

            timeSpawnGap = startTimeBetweenSpawn;

        }
        else
        {
            timeSpawnGap -= Time.deltaTime;
        }


    }

    private RaycastHit2D HandleRaycast(Vector2 direction, float distance)
    {
        RaycastHit2D hit = Physics2D.Raycast(direction.normalized * radius + (Vector2)transform.position, direction, distance, groundLayer);
        return hit;
    }

    private void HandleMovement(bool hitForward, bool hitGroundward, bool hitBackward, Vector2 forward, Vector2 groundward, Vector2 backward)
    {
        if (!hitForward && hitGroundward) //forward  groundward
        {
            rb.velocity = forward * speed;
        }
        else if (!hitGroundward && !hitForward && !hitBackward) // rounding corner groundward forward backward
        {
            rb.AddForce(gravity * speed);
            int sign = movingRight ? 1 : -1;
            rb.velocity = new Vector2(rb.velocity.x + sign * gravity.y * Time.fixedDeltaTime * 1.0f, rb.velocity.y - sign *  gravity.x * Time.fixedDeltaTime * 1.0f);
        }
        else if (hitBackward && !hitGroundward) // exit out of rounding corner
        {
            gravity = backward;
            rb.velocity = backward * speed + groundward * speed;
        }
        else if (hitForward)
        {
            gravity = forward;
        }
    }
}
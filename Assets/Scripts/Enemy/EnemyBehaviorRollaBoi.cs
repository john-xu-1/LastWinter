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
            if (movingRight)
            {
                if (!hitRight && hitDown)
                {
                    rb.AddForce(gravity);
                    rb.velocity = Vector2.right * speed;
                }
                else if(hitRight && hitDown)
                {

                }
                else if (!hitDown && !hitRight && !hitLeft)
                {
                    rb.AddForce(gravity * speed);
                    rb.velocity = new Vector2(rb.velocity.x - Time.fixedDeltaTime * 1.5f, rb.velocity.y);
                }
                else if (hitLeft)
                {
                    gravity = Vector2.left;
                    rb.velocity = Vector2.left * speed + Vector2.down * speed;
                }
            }
            else
            {

            }
        }
        else if (gravity.y > 0)//up
        {
            if (movingRight)
            {
                if (!hitLeft && hitUp)
                {
                    rb.AddForce(gravity);
                    rb.velocity = Vector2.left * speed;
                }
                else if (!hitLeft && !hitUp && !hitRight)
                {
                    rb.AddForce(gravity * speed);
                    rb.velocity = new Vector2(rb.velocity.x + Time.fixedDeltaTime * 1.5f, rb.velocity.y);
                }
                else if (hitRight)
                {
                    gravity = Vector2.right;
                    rb.velocity = Vector2.right * speed + Vector2.up * speed;
                }
            }

        }
        else if (gravity.x < 0)//left
        {
            if (movingRight)
            {
                if (!hitDown && hitLeft)
                {
                    rb.AddForce(gravity);
                    rb.velocity = Vector2.down * speed;
                }
                else if (!hitDown && !hitLeft && !hitUp)
                {
                    Debug.Log("!hitDown && !hitLeft");
                    rb.AddForce(gravity * speed);
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + Time.fixedDeltaTime);
                }
                else if (hitUp)
                {
                    gravity = Vector2.up;
                    rb.velocity = Vector2.left * speed + Vector2.up * speed;
                }
            }
        }
        else if (gravity.x > 0)//right
        {
            if (movingRight)
            {
                if (!hitUp && hitRight)
                {
                    rb.AddForce(gravity);
                    rb.velocity = Vector2.up * speed;
                }
                else if (!hitUp && !hitRight && !hitDown)
                {
                    Debug.Log("!hitDown && !hitLeft");
                    rb.AddForce(gravity * speed);
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - Time.fixedDeltaTime);
                }
                else if (hitDown)
                {
                    gravity = Vector2.down;
                    rb.velocity = Vector2.down * speed + Vector2.right * speed;
                }
            }
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
}


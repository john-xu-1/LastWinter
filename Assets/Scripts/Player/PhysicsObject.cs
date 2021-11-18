using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public Vector2 velocity;

    public float gravityModifer = 1f;

    protected Vector2 targetVelocity;

    protected Rigidbody2D rb;

    protected ContactFilter2D cf;

    protected bool isGrounded;

    protected Vector2 groundNormal;

    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

    public float minGroundNormY = 0.65f;

    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected const float minMoveDistance = 0.001f;

    protected const float padding = 0.01f;



    void Start()
    {

        doOnStart();
    }

    public virtual void doOnStart()
    {
        rb = GetComponent<Rigidbody2D>();

        cf.useTriggers = false;
        cf.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        cf.useLayerMask = true;
    }

    
    void FixedUpdate()
    {

        velocity += gravityModifer * Physics2D.gravity * Time.deltaTime;

        velocity.x = targetVelocity.x;


        isGrounded = false;

        Vector2 groundAffinity = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 move = groundAffinity * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);


    }


    private void Update()
    {
        doOnUpdate();
    }

    protected virtual void doOnUpdate()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {
        
    }

    void Movement (Vector2 mov, bool yMovement)
    {
        float distance = mov.magnitude;

        if (distance > minMoveDistance)
        {
            int count = rb.Cast(mov, cf, hitBuffer, distance + padding);
            hitBufferList.Clear();

            for (int i = 0; i < count; i += 1)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            for(int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 curNorm = hitBufferList[i].normal;

                if (curNorm.y > minGroundNormY)
                {
                    isGrounded = true;
                    if (yMovement)
                    {
                        groundNormal = curNorm;
                        curNorm.x = 0;
                    }
                    
                }

                float projection = Vector2.Dot(velocity, curNorm);

                if (projection < 0)
                {
                    velocity = velocity - projection * curNorm;
                }

                float modDistance = hitBufferList[i].distance - padding;

                distance = modDistance < distance ? modDistance : distance;
            }

        }

        rb.position += mov.normalized * distance;

        
    }

    public static float raycast(Vector2 start, Vector2 direction, float length, LayerMask layerMask)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(start, direction, length, layerMask);
        Debug.DrawRay(start, direction * length, Color.red);
        if (raycastHit2D) return raycastHit2D.distance;
        else return float.MaxValue;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorFlai : EnemyBehaviorBase
{
    public float rotSpeed;
    public float dashSpeed;

    public override void defaultAI()
    {
        Vector2 direction = (Vector2)p.transform.position - rb.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount * rotSpeed;

        rb.velocity = transform.up * speed;
        
        
    }


}

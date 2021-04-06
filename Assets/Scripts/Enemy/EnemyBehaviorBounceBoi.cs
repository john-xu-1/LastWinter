using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorBounceBoi : EnemyBehaviorBase
{
    bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    public override void defaultAI()
    {
        base.defaultAI();

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (isGrounded)
        {
            rb.velocity = Vector2.up * speed;
        }
    }

    

}

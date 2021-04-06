using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorDasher : EnemyBehaviorBase
{
    public float dashSpeed = 20;

    private float nextAttackTime;
    public float fireCD;
    public BoxCollider2D cld;

    public override void defaultAI()
    {
        if (getDistX(p.transform, transform) >= angerRange)
        {
            base.defaultAI();
            rb.drag = 0;
        }
        else
        {
            Vector3 target = p.transform.position;
            Vector2 direction = (target - transform.position).normalized;

            if (Time.time >= nextAttackTime)
            {
                rb.drag = 5;
                cld.enabled = false;
                rb.AddForce(new Vector2(direction.x * dashSpeed, rb.velocity.y), ForceMode2D.Impulse);


                nextAttackTime = Time.time + fireCD;
            }
            else
            {
                cld.enabled = true;
            }


        }


    }
}

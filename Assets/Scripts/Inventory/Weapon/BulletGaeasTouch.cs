using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGaeasTouch : BulletBase
{
    bool isAir = true;
    bool isGrow;

    public float bounceForce = 5;


    public override void setUp()
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (target - transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * speed, direction.y * speed);
        transform.right = GetComponent<Rigidbody2D>().velocity;
    }

    public override void updateBehavior()
    {
        if (isAir)
        {
            transform.eulerAngles += new Vector3(0, 0, transform.right.z * Time.deltaTime);
        }
        if (isGrow)
        {
            transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, 0);
        }
        Destroy(gameObject, weapon.CDIndex);
    }

    public override void collideEnterBehavior(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isGrow = true;
            isAir = false;
            transform.eulerAngles = Vector3.zero;
        }
        
        
    }

}

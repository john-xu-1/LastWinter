using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFlamethrower : BulletBase
{
  

    public override void setUp()
    {

        Vector3 tar = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (tar - transform.position).normalized;


        GetComponent<Rigidbody2D>().AddForce(new Vector2(direction.x * speed, direction.y * speed), ForceMode2D.Impulse);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBreakOfDawn : BulletBase
{
    //private GameObject player;

    public Vector2 ranRange;

    public override void setUp()
    {

        Vector3 tar = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (tar - transform.position).normalized;

        //player = GameObject.FindGameObjectWithTag("Player");

        float ranY = Random.Range(ranRange.x, ranRange.y);

        GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * speed, direction.y * speed + ranY);

    }

    public override void updateBehavior()
    {
        transform.right = GetComponent<Rigidbody2D>().velocity;
    }

    public override void trailBehavior(TrailRenderer tr)
    {
        tr.colorGradient = weapon.chip.trail.GetComponent<TrailRenderer>().colorGradient;
        tr.time = weapon.chip.trail.GetComponent<TrailRenderer>().time;
        tr.sharedMaterials = weapon.chip.trail.GetComponent<TrailRenderer>().sharedMaterials;
    }


}

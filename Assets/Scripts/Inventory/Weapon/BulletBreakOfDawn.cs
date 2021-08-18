using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBreakOfDawn : BulletBase
{
    //private GameObject player;

    public Vector2 ranRange;

    //public bool isSub;

    public override void setUp()
    {

        Vector3 tar = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (tar - transform.position).normalized;

        //player = GameObject.FindGameObjectWithTag("Player");

        float ranY = Random.Range(ranRange.x, ranRange.y);

        int ranBul = Random.Range(0, 4);

        if (ranBul == 0)
        {
            GetComponent<SpriteRenderer>().sprite = weapon.spawnedPrefab1.GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = weapon.spawnedPrefab2.GetComponent<SpriteRenderer>().sprite;
            Debug.Log("yo");
        }

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

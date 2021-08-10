using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletOldShotgun : BulletBase
{
    private GameObject player;

    public int numOfExtra;
    public Vector2 range;

    public GameObject selfPrefab;

    public bool isSub;

    public float recoil = 2f;
    public float recoilDuration = 0.1f;

    Vector3 tar;

    Vector3 direction;

    public override void trailBehavior(TrailRenderer tr)
    {
        tr.colorGradient = weapon.chip.trail.GetComponent<TrailRenderer>().colorGradient;
        tr.widthCurve = weapon.chip.trail.GetComponent<TrailRenderer>().widthCurve;
        
    }


    public override void setUp()
    {
        tar = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = (tar - transform.position).normalized;
        if (isSub)
        {
            float angle = Random.Range(range.x, range.y);

            GetComponent<Rigidbody2D>().velocity = new Vector2(speed * direction.x + angle, speed * direction.y + angle);
            
            transform.right = GetComponent<Rigidbody2D>().velocity;
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
           
            GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * speed, direction.y * speed);

            

            for (int i = 0; i < numOfExtra; i++)
            {
                GameObject instance =  Instantiate(selfPrefab, transform.position, Quaternion.identity);


                Destroy(instance, weapon.destroyAfterTime);
            }
        }
        
        

    }

    public override void updateBehavior()
    {
        
        if (recoilDuration<= 0)
        {
            GameObject.FindGameObjectWithTag("Player").transform.GetComponent<PlatformerController>().specialXForce = 0;
            Debug.Log("diminished");
        }
        else
        {
            recoilDuration -= Time.deltaTime;
            GameObject.FindGameObjectWithTag("Player").transform.GetComponent<PlatformerController>().specialXForce = recoil * -direction.x;
        }
    }

}

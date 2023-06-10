using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BulletGaeasTouch : BulletBase
{

    bool isPlanted = false;

    public Tilemap tilemap;

    public int maxSize = 2;

    public float bounceForce = 5;

    public PlatformerController pc;

    public GameObject coolRay;

    public override void setUp()
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        tilemap = GameObject.FindGameObjectWithTag("Ground").GetComponent<Tilemap>();

        Vector3Int targetTile = tilemap.WorldToCell(target);

        transform.eulerAngles = Vector3.zero;

        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerController>();

       

        if (tilemap.GetTile(targetTile + Vector3Int.up) == null && tilemap.GetTile(targetTile))
        {

            GameObject instance = Instantiate(coolRay, transform.position, Quaternion.identity);

            Destroy(instance, 3f);

            Vector3 direction = (targetTile + Vector3.up * 1.5f - transform.position).normalized;

            instance.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * speed, direction.y * speed);

            transform.position = targetTile + Vector3Int.up;

        }
        else
        {
            pc.specialYForce = 0;
            Destroy(gameObject);

        }



    }


    public override void triggerEnterBehavior(Collider2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            Invoke("delayPlant", 0.5f);
        }

        if (collision.transform.CompareTag("Player"))
        {
            pc.specialYForce += bounceForce;

        }
    }

    public void delayPlant()
    {
        isPlanted = true;
    }



    public override void updateBehavior()
    {
        if (isPlanted && !isFinishGrowTo(2))
        {
            transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, 0);
        }
    }

    public bool isFinishGrowTo(int max)
    {
        if (transform.localScale.x >= max && transform.localScale.y >= max)
        {
            return true;
        }
        return false;
    }

    


    // ===== old Gaeas code =====



    //bool isAir = true;
    //bool isGrow;

    //public float bounceForce = 5;

    //bool isGrown;

    //public float whenFinishGrown = 5;

    //public bool isFall;

    //public float floatingValue;


    //public override void setUp()
    //{
    //    Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    Vector2 direction = (target - transform.position).normalized;
    //    GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * speed, direction.y * speed);
    //    transform.right = GetComponent<Rigidbody2D>().velocity;


    //}

    //public override void updateBehavior()
    //{
    //    if (transform.localScale.x >= whenFinishGrown)
    //    {
    //        isGrown = true;
    //    }


    //    if (isAir)
    //    {
    //        transform.eulerAngles += new Vector3(0, 0, transform.right.z * Time.deltaTime);
    //    }
    //    if (isGrow)
    //    {
    //        if (!isGrown)
    //        {
    //            transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, 0);
    //        }
    //        else
    //        {
    //            isGrow = false;
    //        }



    //        //transform.position = new Vector3(transform.position.x, transform.position.y + scalePosLine(m, transform.localScale.x), 0);

    //    }







    //}

    //bool isBounceOnce;

    //public override void collideEnterBehavior(Collision2D collision)
    //{
    //    if (collision.transform.CompareTag("Ground"))
    //    {
    //        isGrow = true;
    //        isAir = false;
    //        transform.eulerAngles = Vector3.zero;
    //    }
    //}

    //public override void triggerEnterBehavior(Collider2D collision)
    //{

    //    if (collision.transform.CompareTag("Player"))
    //    {
    //        if (isGrown && !isBounceOnce)
    //        {
    //            isBounceOnce = true;
    //            collision.transform.GetComponent<PlatformerController>().specialYForce += bounceForce;

    //        }
    //    }



    //}

    //public override void triggerExitBehavior(Collider2D collision)
    //{

    //    if (collision.transform.CompareTag("Player"))
    //    {
    //        if (collision.transform.GetComponent<PlatformerController>().specialYForce != 0)
    //        {
    //            collision.transform.GetComponent<PlatformerController>().specialYForce -= bounceForce;
    //        }



    //        if (isBounceOnce)
    //        {
    //            Destroy(gameObject);
    //        }
    //    }


    //}




    //public float scalePosLine(float m, float x)
    //{
    //    float y;

    //    y = m * (x - 1);

    //    return y;
    //}



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletVol : BulletBase
{
    public int jumpTimes;
    public Vector2Int JumptRange;
    int hasJumpedTimes;
    public List<GameObject> hasCollided;
    public GameObject subBullet;
    private GameObject player;
    public bool isSub;
    public float KB;



    public override void trailBehavior(TrailRenderer tr)
    {
        tr.colorGradient = weapon.chip.trail.GetComponent<TrailRenderer>().colorGradient;
        tr.widthCurve = weapon.chip.trail.GetComponent<TrailRenderer>().widthCurve;

    }



    public override void setUp()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 tar = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (tar - transform.position).normalized;
        player.GetComponent<Rigidbody2D>().AddForce(-dir * KB, ForceMode2D.Impulse);

        if(!isSub)
        {
            
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (target - transform.position).normalized;
            int ranInt = Random.Range(JumptRange.x, JumptRange.y);
            jumpTimes = ranInt;
            GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * speed, direction.y * speed);
            transform.right = GetComponent<Rigidbody2D>().velocity;

            
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
            GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        }
       
    }

    GameObject GetClosestEnemy(List<GameObject> enemies, Vector2 enemyPos)
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector2 currentPos = enemyPos;
        foreach (GameObject t in enemies)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }

    public override void triggerEnterBehavior(Collider2D collision)
    {
        if (collision.transform.CompareTag("enemy"))
        {
            if (!isSub)
            {
                hasJumpedTimes += 1;
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
                List<GameObject> enemyList = new List<GameObject>();
                for (int i = 0; i < enemies.Length; i++)
                {
                    enemyList.Add(enemies[i]);
                }



                hasCollided.Add(collision.transform.parent.gameObject);

                foreach (GameObject element in hasCollided)
                {
                    enemyList.Remove(element);
                }


                Vector2 curEnemy = collision.transform.position;

                if (hasJumpedTimes < jumpTimes && collision.transform.parent && enemyList.Count > 0)
                {
                    Vector2 direction = (GetClosestEnemy(enemyList, curEnemy).transform.position - transform.position).normalized;
                    GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * speed, direction.y * speed);
                    transform.right = GetComponent<Rigidbody2D>().velocity;

                }



            }



        }
    }
    public override void triggerExitBehavior(Collider2D collision)
    {
        base.triggerExitBehavior(collision);
        
        if (collision.transform.CompareTag("enemy"))
        {
            if (!isSub)
            {
                GameObject Instance = Instantiate(subBullet, gameObject.transform.position, gameObject.transform.rotation);
                Destroy(Instance, weapon.destroyAfterTime);
            }
        }
           
    }

    



}

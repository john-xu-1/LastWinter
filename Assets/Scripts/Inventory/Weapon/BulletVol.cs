using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BulletVol : BulletBase
{
    public Tilemap tilemap;


    public override void setUp()
    {
        Vector3 tar = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (tar - transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * speed, direction.y * speed);

        tilemap = GameObject.FindGameObjectWithTag("Ground").GetComponent<Tilemap>();
    }

    public override void triggerEnterBehavior(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            Vector3Int pos = tilemap.WorldToCell(transform.position);
            tilemap.SetTile(pos, null);
            tilemap.SetTile(pos + Vector3Int.up, null);
            tilemap.SetTile(pos + Vector3Int.down, null);
            tilemap.SetTile(pos + Vector3Int.left, null);
            tilemap.SetTile(pos + Vector3Int.right, null);
            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    
    




    // ======= old vol behavior =======

    //public int jumpTimes;
    //public Vector2Int JumptRange;
    //int hasJumpedTimes;
    //public List<GameObject> hasCollided;
    //public GameObject subBullet;
    //private GameObject player;
    //public bool isSub;
    //public float KB;



    //public override void trailBehavior(TrailRenderer tr)
    //{
    //    tr.colorGradient = weapon.chip.trail.GetComponent<TrailRenderer>().colorGradient;
    //    tr.widthCurve = weapon.chip.trail.GetComponent<TrailRenderer>().widthCurve;

    //}



    //public override void setUp()
    //{
        
    //    player = GameObject.FindGameObjectWithTag("Player");
    //    Vector3 tar = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    Vector2 dir = (tar - transform.position).normalized;
    //    player.GetComponent<Rigidbody2D>().AddForce(-dir * KB, ForceMode2D.Impulse);

    //    if(!isSub)
    //    {
            
    //        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        Vector2 direction = (target - transform.position).normalized;
    //        int ranInt = Random.Range(JumptRange.x, JumptRange.y);
    //        jumpTimes = ranInt;
    //        GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * speed, direction.y * speed);
    //        transform.right = GetComponent<Rigidbody2D>().velocity;

            
    //    }
    //    else
    //    {
    //        transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
    //        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    //    }
       
    //}

    //GameObject GetClosestEnemy(List<GameObject> enemies, Vector2 enemyPos)
    //{
    //    GameObject tMin = null;
    //    float minDist = Mathf.Infinity;
    //    Vector2 currentPos = enemyPos;
    //    foreach (GameObject t in enemies)
    //    {
    //        float dist = Vector3.Distance(t.transform.position, currentPos);
    //        if (dist < minDist)
    //        {
    //            tMin = t;
    //            minDist = dist;
    //        }
    //    }
    //    return tMin;
    //}

    //public override void triggerEnterBehavior(Collider2D collision)
    //{
    //    if (collision.transform.CompareTag("enemy"))
    //    {
    //        if (!isSub)
    //        {
    //            hasJumpedTimes += 1;
    //            GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
    //            List<GameObject> enemyList = new List<GameObject>();
    //            for (int i = 0; i < enemies.Length; i++)
    //            {
    //                enemyList.Add(enemies[i]);
    //            }



    //            hasCollided.Add(collision.gameObject);

    //            foreach (GameObject element in hasCollided)
    //            {
    //                enemyList.Remove(element);
    //            }


    //            Vector2 curEnemy = collision.transform.position;

    //            if (hasJumpedTimes < jumpTimes && collision && enemyList.Count > 0)
    //            {
    //                Vector2 direction = (GetClosestEnemy(enemyList, curEnemy).transform.position - transform.position).normalized;
    //                GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * speed, direction.y * speed);
    //                transform.right = GetComponent<Rigidbody2D>().velocity;

    //            }



    //        }



    //    }
    //}
    //public override void triggerExitBehavior(Collider2D collision)
    //{
    //    base.triggerExitBehavior(collision);
        
    //    if (collision.transform.CompareTag("enemy"))
    //    {
    //        if (!isSub)
    //        {
    //            GameObject Instance = Instantiate(subBullet, gameObject.transform.position, gameObject.transform.rotation);
    //            Destroy(Instance, weapon.destroyAfterTime);
    //        }
    //    }
           
    //}

    



}

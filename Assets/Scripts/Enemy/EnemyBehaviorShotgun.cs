using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorShotgun : EnemyBehaviorBase
{
    public GameObject bulletPrefab;
    private float nextShootTime;
    
    public float fireCD;

    public int amountOfBullets = 3;

    public Vector2 minMax;

    public float bulletSpeed;
    public GameObject[] instances;

    public float destroyTime = 2;


    public override void defaultAI()
    {

        if (getDistX(p.transform, transform) <= angerRange)
        {
            if (Time.time >= nextShootTime)
            {
                for(int i = 0; i < amountOfBullets; i += 1)
                {
                    instances[i] = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                }

                foreach(GameObject instance in instances)
                {
                    Vector3 target = p.transform.position;
                    Vector2 direction = (target - transform.position).normalized;
                    float j = Random.Range(minMax.x, minMax.y);
                    direction = new Vector2(direction.x + j, direction.y + j);
                    instance.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed * direction.x, bulletSpeed * direction.y);
                    Destroy(instance, destroyTime);
                }
                
                nextShootTime = Time.time + fireCD;
            }
        }
        else
        {
            Vector3 target = p.transform.position;
            Vector2 direction = ((target - transform.position) * Vector2.right).normalized;


            Vector2 vel = new Vector2(direction.x * speed, 0);


            rb.velocity = vel;
                
            Debug.Log(vel);
        }
    }

    public override void inflictDamage()
    {
        
    }
}

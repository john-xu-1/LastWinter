using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorShotgun : EnemyBehaviorBase
{
    public GameObject bulletPrefab;
    private float nextAttackTime;
    
    public float fireCD;

    public int amountOfBullets = 3;

    public Vector2 minMax;

    public float bulletSpeed;
    public GameObject[] instances;


    public override void defaultAI()
    {

        if (getDistX(p.transform, transform) <= angerRange)
        {
            if (Time.time >= nextAttackTime)
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
                    Destroy(instance, 5);
                }
                
                nextAttackTime = Time.time + fireCD;
            }
        }
    }
}

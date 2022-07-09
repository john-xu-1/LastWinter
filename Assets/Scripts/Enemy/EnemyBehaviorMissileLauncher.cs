using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorMissileLauncher : EnemyBehaviorBase
{
    public GameObject bulletPrefab;
    private float nextShootTime;

    public float fireCD;

    public float destroyTime;

    GameObject instance;


    public override void defaultAI()
    {

        if (getDistX(p.transform, transform) <= angerRange)
        {
            if (Time.time >= nextShootTime)
            {
                instance = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                
                
                Destroy(instance, destroyTime);

                nextShootTime = Time.time + fireCD;
            }
        }
        
    }

    public override void inflictDamage()
    {

    }
}

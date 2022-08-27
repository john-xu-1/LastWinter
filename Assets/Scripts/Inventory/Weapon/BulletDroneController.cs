using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDroneController : BulletBase
{
    public int cdMinus = 2;
    public int collisionRequiredTimes = 60;
    int collidedEnemyTimes;
    bool collisionCheck = true;
    public override void updateBehavior()
    {
        base.updateBehavior();
        if (weapon.weaponType == InventoryWeapon.WeaponTypes.Drone_Controller)
        {
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (target - transform.position).normalized;
            GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * speed, direction.y * speed);

            speed += (Time.deltaTime * 5);

            Debug.Log(collisionCheck);
        }
    }

    public override void triggerEnterBehavior(Collider2D collision)
    {
        base.triggerEnterBehavior(collision);
        if (collision.gameObject.layer == 13)
        {
            if (collisionCheck)
            {
                collidedEnemyTimes += 1;
                if (collidedEnemyTimes >= collisionRequiredTimes)
                {
                    Destroy(gameObject);
                    
                }
                
                Debug.Log(weapon.curAttackTime);
                collisionCheck = false;
            }
            

        }

        
    }

    public override void collideEnterBehavior(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            if (speed >= 0) speed -= 5;
        }
    }


    public override void triggerExitBehavior(Collider2D collision)
    {
        base.triggerExitBehavior(collision);
        collisionCheck = true;
    }

    public override void trailBehavior(TrailRenderer tr)
    {
        tr.colorGradient = weapon.chip.trail.GetComponent<TrailRenderer>().colorGradient;
        tr.widthCurve = weapon.chip.trail.GetComponent<TrailRenderer>().widthCurve;
        tr.widthMultiplier = weapon.chip.trail.GetComponent<TrailRenderer>().widthMultiplier;
    }
}

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

        }
    }

    public override void triggerEnterBehavior(Collider2D collision)
    {
        base.triggerEnterBehavior(collision);
        if (collision.transform.CompareTag("enemy"))
        {
            if (collisionCheck)
            {
                collidedEnemyTimes += 1;
                if (collidedEnemyTimes >= collisionRequiredTimes)
                {
                    Destroy(gameObject);
                    
                }
                weapon.curAttackTime = weapon.curAttackTime - cdMinus;
                collisionCheck = false;
            }
            

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

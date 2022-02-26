using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMagnetizedShifter : BulletBase
{

    public override void setUp()
    {
        base.setUp();
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (target - transform.position).normalized;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(direction.x * speed, direction.y * speed), ForceMode2D.Impulse);
    }

    public override void updateBehavior()
    {
        base.updateBehavior();
        GetComponent<Rigidbody2D>().drag += Time.deltaTime * (1 / speed);
    }

    public override void triggerEnterBehavior(Collider2D collision)
    {
        
        if (collision.transform.CompareTag("bullet"))
        {

            SpriteRenderer sr = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
            SpriteRenderer sr2 = collision.transform.GetComponent<SpriteRenderer>();

            Vector2 direction = (collision.transform.position - transform.position).normalized;
            if (sr.sprite == sr2.sprite)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-direction.x * speed * 2, -direction.y * speed * 2);
                Debug.Log("Repel");
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * speed * 2, direction.y * speed * 2);
                Debug.Log("Attract");
            }
        }
        if(collision.transform.CompareTag("enemy"))
        {
            if (weapon.weaponType == InventoryWeapon.WeaponTypes.Magnetized_Shifter)
            {

                Vector2 direction = GetComponent<Rigidbody2D>().velocity.normalized;
                collision.transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * speed * 5, direction.y * speed * 5);
            }
        }
    }
    public override void triggerExitBehavior(Collider2D collision)
    {
        base.triggerExitBehavior(collision);
        if (collision.transform.parent.CompareTag("bullet"))
        {
            if (collision.transform.parent.gameObject.GetComponent<BulletMagnetizedShifter>().weapon.weaponType == InventoryWeapon.WeaponTypes.Magnetized_Shifter)
            {

                Destroy(gameObject, 0.5f);
                Destroy(collision.transform.parent.gameObject, 0.5f);
            }
        }
    }
}


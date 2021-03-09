using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRainyDay : BulletBase
{
    public override void setUp()
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (target - transform.position).normalized;
        if (weapon.weaponType == InventoryWeapon.WeaponTypes.Rainy_Day)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * speed, direction.y * speed);
            transform.right = GetComponent<Rigidbody2D>().velocity;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFlashlight : BulletBase
{
    public override void setUp()
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (target - transform.position).normalized;
        transform.right = direction;
    }
    public override void updateBehavior()
    {
        if(FindObjectOfType<InventorySystem>().weapon.weaponType == InventoryWeapon.WeaponTypes.Flash_Light)
        {
            gameObject.SetActive(true);
            transform.parent = GameObject.FindGameObjectWithTag("SelectedItem").transform;
        }
        else
        {
            gameObject.SetActive(false);
        }
        
        Destroy(gameObject, weapon.CDIndex);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase: MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    public InventoryWeapon weapon;
    public GameObject ExplosionPrefab;
    void Setup(Rigidbody2D rb, Transform t)
    {
        if(weapon.weaponType == InventoryWeapon.WeaponTypes.Rainy_Day)
        {
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (target - t.position).normalized;
            rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
            transform.right = rb.velocity;
        }
       
    }

    private void Start()
    {
        weapon = (InventoryWeapon)FindObjectOfType<InventorySystem>().selectedItem;
        rb = GetComponent<Rigidbody2D>();
        Setup(rb, transform);
    }

    
}

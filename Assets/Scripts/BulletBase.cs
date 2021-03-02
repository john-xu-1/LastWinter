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
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (target - t.position).normalized;
        
        if (weapon.weaponType == InventoryWeapon.WeaponTypes.Rainy_Day)
        {
            rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
            transform.right = rb.velocity;
        }
        if (weapon.weaponType == InventoryWeapon.WeaponTypes.Magnetized_Shifter)
        {
            rb.AddForce(new Vector2(direction.x * speed, direction.y * speed), ForceMode2D.Impulse);
        }
       
    }

    private void Start()
    {
        weapon = (InventoryWeapon)FindObjectOfType<InventorySystem>().selectedItem;
        rb = GetComponent<Rigidbody2D>();
        Setup(rb, transform);
    }

    private void Update()
    {
        if (weapon.weaponType == InventoryWeapon.WeaponTypes.Magnetized_Shifter)
        {
            rb.drag += Time.deltaTime * (1/speed);
        }

    }


}

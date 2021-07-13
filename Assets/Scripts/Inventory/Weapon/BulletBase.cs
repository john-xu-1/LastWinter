using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase: MonoBehaviour
{
    public float speed;
    public InventoryWeapon weapon;
    public GameObject ExplosionPrefab;
    public GameObject FireExplosionPrefab;

   

    private void Start()
    {
        weapon = (InventoryWeapon)FindObjectOfType<InventorySystem>().selectedItem;
        setUp();
    }

    private void Update()
    {
        
        updateBehavior();

    }

    private void FixedUpdate()
    {
        fixedUpdateBehavior();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        triggerEnterBehavior(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
        triggerExitBehavior(collision);

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collideEnterBehavior(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        collideStayBehavior(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collideExitBehavior(collision);
    }

    public virtual void triggerEnterBehavior(Collider2D collision)
    {
        triggerStayBehavior(collision);
    }


    public virtual void triggerExitBehavior(Collider2D collision)
    {

    }

    public virtual void updateBehavior()
    {

    }

    public virtual void setUp()
    {

    }

    public virtual void triggerStayBehavior(Collider2D collision)
    {

    }

    public virtual void collideEnterBehavior(Collision2D collision)
    {

    }

    public virtual void collideStayBehavior(Collision2D collision)
    {

    }

    public virtual void collideExitBehavior(Collision2D collision)
    {

    }

    public virtual void fixedUpdateBehavior()
    {

    }

}

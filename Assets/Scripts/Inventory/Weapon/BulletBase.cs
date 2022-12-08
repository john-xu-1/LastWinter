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
        if (ExplosionPrefab) Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        weapon = (InventoryWeapon)FindObjectOfType<InventorySystem>().selectedItem;

        
        TrailRenderer tr = transform.GetChild(0).GetComponent<TrailRenderer>();
        if (weapon.chip)
        {
            trailBehavior(tr);
        }
        
        setUp();
    }

    private void Update()
    {
        
        updateBehavior();

    }

    public virtual void trailBehavior(TrailRenderer tr)
    {
        tr.colorGradient = weapon.chip.trail.GetComponent<TrailRenderer>().colorGradient;
        tr.widthCurve = weapon.chip.trail.GetComponent<TrailRenderer>().widthCurve;
        tr.widthMultiplier = weapon.chip.trail.GetComponent<TrailRenderer>().widthMultiplier;
        tr.time = weapon.chip.trail.GetComponent<TrailRenderer>().time;
        tr.sharedMaterials = weapon.chip.trail.GetComponent<TrailRenderer>().sharedMaterials;
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

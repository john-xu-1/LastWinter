using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public InventoryObjects item;
    public SpriteRenderer sr;

    private Rigidbody2D rb;
    

    public Vector2 hitSize = new Vector2 (1,1);

    public Vector2 rangeSize = new Vector2 (4f, 4f);

    public float attractionSpeed = 5;

    public float torque = 5;

    private float orgTorq;

    public bool startChase;
    public Collider2D targ;

    private InventorySystem inventorySystem;


    private void Start()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        sr.sprite = item.itemSprite;
        orgTorq = torque;
        rb = GetComponent<Rigidbody2D>();

    }
    private void Update()
    {

        if (inventorySystem == null) inventorySystem = FindObjectOfType<InventorySystem>();

        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, hitSize, 0f);

        Collider2D[] ranges = Physics2D.OverlapBoxAll(transform.position, rangeSize, 0f);


        foreach (Collider2D range in ranges)
        {
            if (range.transform.CompareTag("Player"))
            {
                startChase = true;

                targ = range;

            }
            else
            {
                //targ = null;
                startChase = false;
            }
        }


        if (targ)
        {
            float dist = Vector2.Distance(transform.position, targ.transform.position);
            if (dist > rangeSize.x)
            {
                rb.velocity = Vector2.zero;
                torque = 0f;
            }
        }
        
        
        if (inventorySystem.isInvFull == false && startChase)
        {
            chase(targ);
            torque = orgTorq;
            Debug.Log("chase");
        }

        if (inventorySystem.isInvFull == true && startChase)
        {
            rb.velocity = Vector2.zero;
            startChase = false;
            torque = 0f;
        }

        foreach (Collider2D hit in hits)
        {
            if (hit.transform.CompareTag("Player"))
            {
                startChase = false;
                if (!inventorySystem.isInvFull)
                {
                    inventorySystem.AddItem(item);
                    Destroy(gameObject);
                }

                break;

            }
        }
        
        


    }

    public void setInventorySystem(InventorySystem inventorySystem)
    {
        this.inventorySystem = inventorySystem;
    }

    void chase(Collider2D target)
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * attractionSpeed, direction.y * attractionSpeed);
        rb.AddTorque(torque);
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public InventoryObjects item;
    private SpriteRenderer sr;

    private Rigidbody2D rb;
    

    public Vector2 hitSize = new Vector2 (1,1);

    public Vector2 rangeSize = new Vector2 (4f, 4f);

    public float attractionSpeed = 5;

    public float torque = 5;

    private float orgTorq;

    public bool startChase;
    Collider2D targ;

    


    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = item.itemSprite;
        orgTorq = torque;
        rb = GetComponent<Rigidbody2D>();

    }
    private void Update()
    {
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

                startChase = false;
            }
        }

        if (FindObjectOfType<InventorySystem>().isInvFull == false && startChase)
        {
            chase(targ);
            torque = orgTorq;
            Debug.Log("chase");
        }

        if (FindObjectOfType<InventorySystem>().isInvFull == true && startChase)
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
                if (!FindObjectOfType<InventorySystem>().isInvFull)
                {
                    FindObjectOfType<InventorySystem>().AddItem(item);
                    Destroy(gameObject);
                }

            }
        }
        
        


    }

    void chase(Collider2D target)
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * attractionSpeed, direction.y * attractionSpeed);
        rb.AddTorque(torque);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorBase : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D rb;
    public float angerRange = 2;
    public float speed = 5;

    float nextAttackTime;

    public float attackCD = 2;


    public DoDamage dd;

    

    

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        ChildStart();
    }

    

    void FixedUpdate()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            rb = GetComponent<Rigidbody2D>();
            ChildStart();
        }
        if (player && getDistX(player.transform, transform) <= angerRange && getDistY(player.transform, transform) <= angerRange)
        {
            defaultAI();
        }

        

        //inflictDamage();

    }


    protected virtual void ChildStart() { }

    public virtual void defaultAI()
    {
        Vector3 target = player.transform.position;
        Vector2 direction = ((target - transform.position) * Vector2.right).normalized;

        rb.AddForce(new Vector2(direction.x * 5, rb.velocity.y),ForceMode2D.Force);

        //Debug.Log(direction);

    }

    public virtual void inflictDamage()
    {
        if (Time.time >= nextAttackTime)
        {
            dd.DamageInflict();

            nextAttackTime = Time.time + attackCD;
        }
        
        
    }

    public int getDistX(Transform self)
    {
        float dif = player.transform.position.x - self.position.x;

        if (dif > 0)
        {
            return 1;
        }
        else if (dif < 0)
        {
            return -1;
        }
        else
        {
            return 0;
        }
        
    }

    public float getDistX(Transform player, Transform self)
    {
        float dif;
        dif = Mathf.Abs(player.position.x - self.position.x);
        return dif;
    }
    public float getDistY(Transform player, Transform self)
    {
        float dif;
        dif = Mathf.Abs(player.position.y - self.position.y);
        return dif;
    }
}

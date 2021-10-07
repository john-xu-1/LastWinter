using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorBase : MonoBehaviour
{
    public GameObject p;
    public Rigidbody2D rb;
    public float angerRange = 2;
    public float speed = 5;

    public float damage = 5;

    public float attackRange = 1;

    public Transform hitPoint;

    public LayerMask whichcanhit;

    float nextAttackTime;

    public float attackCD = 2;

    

    void Start()
    {
        p = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        ChildStart();
    }

    void Update()
    {
        defaultAI();
        inflictDamage();

    }

    protected virtual void ChildStart() { }

    public virtual void defaultAI()
    {
        Vector3 target = p.transform.position;
        Vector2 direction = (target - transform.position).normalized;

        rb.AddForce(new Vector2(direction.x * 5, rb.velocity.y),ForceMode2D.Force);

        

    }

    public virtual void inflictDamage()
    {
        if (Time.time >= nextAttackTime)
        {
            bool isHit = Physics2D.OverlapCircle(hitPoint.position, attackRange, whichcanhit);

            if (isHit)
            {
                toDamage(damage);
            }

            nextAttackTime = Time.time + attackCD;
        }
        
        
    }

    public virtual void toDamage(float damage)
    {
        if (p.GetComponent<HealthBase>())
        {
            p.GetComponent<HealthBase>().TakeDamage(damage, "Enemy" );
        }
        else
        {
            Debug.Log("healthbase not found");
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

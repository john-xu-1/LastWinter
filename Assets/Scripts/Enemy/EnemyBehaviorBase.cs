using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorBase : MonoBehaviour
{
    public GameObject p;
    public Rigidbody2D rb;
    public float angerRange = 2;
    public float speed = 5;

    float nextAttackTime;

    public float attackCD = 2;


    public DoDamage dd;

    

    void Start()
    {
        p = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        ChildStart();
    }

    void Update()
    {
        if (p) defaultAI();
        //inflictDamage();

    }

    protected virtual void ChildStart() { }

    public virtual void defaultAI()
    {
        Vector3 target = p.transform.position;
        Vector2 direction = ((target - transform.position) * Vector2.right).normalized;

        rb.AddForce(new Vector2(direction.x * 5, rb.velocity.y),ForceMode2D.Force);

        Debug.Log(direction);

    }

    public virtual void inflictDamage()
    {
        if (Time.time >= nextAttackTime)
        {
            dd.DamageInflict();

            nextAttackTime = Time.time + attackCD;
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

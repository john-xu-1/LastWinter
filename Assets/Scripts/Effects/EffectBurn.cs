using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBurn : EffectBase
{
    public float damageRate;
    private float drMax;

    public override void startBehavior()
    {
        base.startBehavior();
        drMax = damageRate;
    }
    

    public override void onTriggerEnterBehavior(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            FindObjectOfType<Effectable>().addEffect(GetComponent<EffectBurn>());
            
        }
    }

    public override void onTriggerStayBehavior(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            duration = maxDuration;
        }
    }

    

    public override void effectBehavior()
    {
        

        if (duration > 0)
        {
            duration -= Time.deltaTime;
            if (!ph.isDead)
            {
                if (damageRate <= 0)
                {
                    ph.TakeDamage(magnitude, "Effect");

                    damageRate = drMax;

                }
                else
                {
                    damageRate -= Time.deltaTime;

                }
            }
        }
        
        
    }
}

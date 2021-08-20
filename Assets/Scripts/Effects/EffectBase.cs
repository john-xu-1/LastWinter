using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EffectBase : MonoBehaviour
{
    public float duration = 1;
    public float maxDuration;

    public float magnitude;

    public PlatformerController pc;

    public HealthPlayer ph;


    private void Start()
    {
        startBehavior();
    }

    public virtual void startBehavior()
    {
        maxDuration = duration;
    }

    public virtual void effectBehavior()
    {
        duration -= Time.deltaTime;
    }

    public virtual void unDo()
    {

    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        onTriggerStayBehavior(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onTriggerEnterBehavior(collision);
    }

    public virtual void onTriggerEnterBehavior(Collider2D collision)
    {

    }

    public virtual void onTriggerStayBehavior (Collider2D collision)
    {

    }

}

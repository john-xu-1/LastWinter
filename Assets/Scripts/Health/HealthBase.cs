using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBase : MonoBehaviour
{
    float MaxHealth = 0;

    public float health = 20;

    public bool isDead;


    public float RegenMagnitude;

    private void Awake()
    {
        doOnAwake();
    }

    public virtual void doOnAwake()
    {
        MaxHealth = health;
    }

    public float GetMaxHealth()
    {
        return MaxHealth;
    }

    public void SetMaxHealth(float max)
    {
        MaxHealth = max;
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
    }

    private void Update()
    {
        if (health <= 0)
        {
            isDead = true;
            death();
        }

        if (health < MaxHealth && isDead == false)
        {
            regen();
        }
    }

    public virtual void regen()
    {
        health += regenLine(RegenMagnitude, Time.deltaTime);
    }

    public virtual float regenLine(float m, float x)
    {
        float y;

        y = m * x;

        return y;
    }

    public virtual void death()
    {
        Debug.Log("base die");
    }



}

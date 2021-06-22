using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBase : MonoBehaviour
{
    float MaxHealth = 0;

    public float health = 20;


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

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
    }

    private void Update()
    {
        if (health <= 0)
        {
            death();
        }
    }

    public virtual void death()
    {
        Debug.Log("base die");
    }



}

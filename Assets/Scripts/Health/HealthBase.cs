using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBase : MonoBehaviour
{
    float MaxHealth = 0;

    float orgMaxHealth = 0;

    public float health = 20;

    public bool isDead;

    public float combatCD = 5f;
    public float nextCombatTime;


    public float RegenMagnitude;

    public bool isCombat;

    private string curDamageType;

    bool regGapHealth = false;

    private void Awake()
    {
        doOnAwake();
    }

    public virtual void doOnAwake()
    {
        MaxHealth = health;
        orgMaxHealth = MaxHealth;
    }

    public void revertToOrgMax()
    {
        SetMaxHealth(orgMaxHealth);
    }

    public float GetMaxHealth()
    {
        return MaxHealth;
    }

    public void SetMaxHealth(float max)
    {
        MaxHealth = max;
    }

    public virtual void TakeDamage(float damage, string type)
    {
        health -= damage;
        curDamageType = type;
    }

    private void Update()
    {
        if (health <= 0)
        {
            isDead = true;
            death();
            health = 0;
        }

        if (curDamageType == "Enemy" || curDamageType == "Effect")
        {
            if (Time.time >= nextCombatTime)
            {
                nextCombatTime = Time.time + combatCD;
                isCombat = false;
                curDamageType = "";
            }
            else
            {
                isCombat = true;
                
            }
        }

        if (health > MaxHealth)
        {
            health = MaxHealth;
        }

        if (health >= orgMaxHealth && health < MaxHealth)
        {
            regGapHealth = true;
        }
        else
        {
            regGapHealth = false;
        }


        if ((health < MaxHealth && isDead == false && isCombat == false) || regGapHealth == true)
        {
            regen();
        }
    }

    public virtual void regen()
    {
        health += RegenMagnitude * Time.deltaTime;

        if (health > MaxHealth)
        {
            health = MaxHealth;
        }
    }

    /*
    public virtual float regenLine(float m, float x)
    {
        float y;

        y = m * x;

        return y;
    }
    */

    public virtual void death()
    {
        Debug.Log("base die");
    }

    



}

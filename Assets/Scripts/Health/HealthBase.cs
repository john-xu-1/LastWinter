using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBase : MonoBehaviour
{
    protected float MaxHealth = 0;

    //protected float orgMaxHealth = 0;

    public float health = 20;

    public bool isDead;

    public float combatCD = 5f;
    protected float nextCombatTime;


    public float RegenMagnitude;

    [SerializeField] protected bool isCombat = false;

    private string curDamageType;

    

    private void Awake()
    {
        doOnAwake();
        displayHealth();
    }

    public virtual void doOnAwake()
    {
        MaxHealth = health;
        //orgMaxHealth = MaxHealth;
    }

    public void revertToOrgMax()
    {
        //SetMaxHealth(orgMaxHealth);
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

        displayHealth();
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
//                Debug.Log("not combat");
                curDamageType = "";
            }
            else
            {
//                Debug.Log("combat");
                isCombat = true;
                
            }
        }

        //if (health > MaxHealth)
        //{
        //    health = MaxHealth;
        //}

       

        childUpdate();
    }

    protected virtual void childUpdate()
    {

    }

    protected virtual void displayHealth()
    {

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

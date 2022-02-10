using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlayer : HealthBase
{
    //bool regGapHealth = false;
    private void Start()
    {
        FindObjectOfType<UIHandler>().setOrgHealth(MaxHealth);
    }

    public override void death()
    {
        FindObjectOfType<GameHandler>().PlayerDied(gameObject);
    }

    protected override void displayHealth()
    {
        FindObjectOfType<UIHandler>().PlayerHealth = health;
        FindObjectOfType<UIHandler>().DisplayPlayerHealth();
    }

    protected override void ChildUpdate()
    {
        //if (health >= orgMaxHealth && health < MaxHealth)
        //{
        //    regGapHealth = true;
        //}
        //else
        //{
        //    regGapHealth = false;
        //}

        if ((health < MaxHealth && isDead == false && isCombat == false) /* || regGapHealth == true */)
        {
            regen();
        }
    }

    public virtual void regen()
    {
        health += RegenMagnitude * Time.deltaTime;
        displayHealth();

        if (health > MaxHealth)
        {
            health = MaxHealth;
        }
    }
}

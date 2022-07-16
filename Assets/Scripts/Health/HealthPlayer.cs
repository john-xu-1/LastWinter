using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlayer : HealthBase
{
    //bool regGapHealth = false;

    public bool isLab = false;

    public GameObject GH;

    private void Start()
    {
        if (!isLab) FindObjectOfType<UIHandler>().setPlayerOrgHealth(MaxHealth);

        GH = GameObject.FindGameObjectWithTag("GameHandler");
    }

    public override void death()
    {
        GH.GetComponent<GameHandler>().PlayerDied(gameObject);

        Debug.Log("deaht");
    }

    protected override void displayHealth()
    {
        if (!isLab)
        {
            FindObjectOfType<UIHandler>().PlayerHealth = health;
            FindObjectOfType<UIHandler>().DisplayPlayerHealth();
        }
        
    }

    protected override void childUpdate()
    {
        //if (health >= orgMaxHealth && health < MaxHealth)
        //{
        //    regGapHealth = true;
        //}
        //else
        //{
        //    regGapHealth = false;
        //}

        if (health < MaxHealth && isDead == false && isCombat == false/* || regGapHealth == true*/)
        {
            regen();
        }
    }

    public void regen()
    {
        health += RegenMagnitude * Time.deltaTime;
        displayHealth();

        if (health > MaxHealth)
        {
            health = MaxHealth;
        }
    }
}

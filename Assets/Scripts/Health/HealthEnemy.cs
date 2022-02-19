using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEnemy : HealthBase
{
    UIEnemyHealthHandler handler;

    public override void death()
    {
        Destroy(gameObject);
    }

    public override void doOnAwake()
    {
        base.doOnAwake();
        handler = GetComponent<UIEnemyHealthHandler>();
    }

    private void Start()
    {
        
        FindObjectOfType<UIHandler>().setEnemyOrgHealth(MaxHealth, handler);
    }

    protected override void displayHealth()
    {
        FindObjectOfType<UIHandler>().DisplayEnemyHealth(health, handler);
    }
}

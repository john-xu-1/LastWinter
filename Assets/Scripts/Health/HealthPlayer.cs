using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlayer : HealthBase
{
    public override void death()
    {
        FindObjectOfType<GameHandler>().PlayerDied(gameObject);

    }

    protected override void displayHealth()
    {
        FindObjectOfType<UIHandler>().PlayerHealth = health;
        FindObjectOfType<UIHandler>().DisplayPlayerHealth();
    }
}

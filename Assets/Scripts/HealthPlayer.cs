using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlayer : HealthBase
{
    public override void death()
    {
        Debug.Log("Player die");

    }
}

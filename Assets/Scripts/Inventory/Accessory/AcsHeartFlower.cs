using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcsHeartFlower : AcsBase
{
    

    public override void setUp()
    {
        HealthPlayer h = player.GetComponent<HealthPlayer>();
        h.SetMaxHealth(h.GetMaxHealth() + magnitude);


    }

    public override void unDo()
    {
        HealthPlayer h = player.GetComponent<HealthPlayer>();
        h.SetMaxHealth(h.GetMaxHealth() - magnitude);
    }



}

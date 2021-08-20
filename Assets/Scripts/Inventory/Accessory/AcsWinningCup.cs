using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcsWinningCup : EffectBase
{

    float orgspeed = 0;

    

    public override void effectBehavior()
    {
        
        if (duration > 0)
        {
            if (orgspeed == 0) orgspeed = pc.MaxSpeedNormal;
            duration -= Time.deltaTime;
            if (!ph.isDead)
            {
                pc.MaxSpeedNormal = magnitude;
                
            }
        }
        else
        {
            unDo();
        }
       
    }

    public override void unDo()
    {
        pc.MaxSpeedNormal = orgspeed;

        FindObjectOfType<Effectable>().effects.RemoveAt(FindObjectOfType<Effectable>().effects.IndexOf(GetComponent<AcsWinningCup>()));
        Destroy(gameObject);
    }
}

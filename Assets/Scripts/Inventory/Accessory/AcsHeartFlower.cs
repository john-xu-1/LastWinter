using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcsHeartFlower : EffectBase
{
    

    public override void effectBehavior()
    {

        if (duration > 0)
        {
            duration -= Time.deltaTime;
            if (!ph.isDead)
            {
                ph.SetMaxHealth(magnitude);
            }
        }
        else
        {
            unDo();
        }
        


    }

    public override void unDo()
    {

        //ph.revertToOrgMax();

        FindObjectOfType<Effectable>().effects.RemoveAt(FindObjectOfType<Effectable>().effects.IndexOf(GetComponent<AcsHeartFlower>()));
        Destroy(gameObject);
    }



}

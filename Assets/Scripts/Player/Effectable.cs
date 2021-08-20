using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effectable : MonoBehaviour
{
    
    public List<EffectBase> effects;


    private void Start()
    {
        
    }

    public void addEffect(EffectBase e)
    {
        effects.Add(e);
        foreach (EffectBase effect in effects)
        {
            effect.pc = GetComponent<PlatformerController>();
            effect.ph = GetComponent<HealthPlayer>();
        }
        
    }

    private void Update()
    {
        if (effects.Count > 0)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                
                if (effects[i])
                {
                    effects[i].effectBehavior();
                }
                
            }
            
        }

        if (effects.Contains(null))
        {
            effects.RemoveAll(null);
        }

        

        
    }


}

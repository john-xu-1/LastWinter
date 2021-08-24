using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EffectDrown : EffectBase
{
    public float damageRate;
    private float drMax;


    public bool isOutWater = true;


    public float timer;

    public override void startBehavior()
    {
        base.startBehavior();
        drMax = damageRate;
    }


    public override void updateBehavior()
    {
        if (isOutWater)
        {
            if (duration < maxDuration)
            {
                duration += Time.deltaTime;
            }
            else
            {
                duration = maxDuration;
            }
            
        }
        
    }


    public override void onTriggerStayBehavior(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            TileBase tileBase = UtilityTilemap.GetTile(GetComponent<Tilemap>(), collision.transform.position);

            if (tileBase)
            {
                if (tileBase.name == "LW_WaterBottomTile")
                {
                    if (!FindObjectOfType<Effectable>().effects.Contains(GetComponent<EffectDrown>()))
                    {
                        FindObjectOfType<Effectable>().addEffect(GetComponent<EffectDrown>());
                    }
                    isOutWater = false;
                    Debug.Log("In water");


                }
                else
                {
                    isOutWater = true;
                }
                
            }
            else
            {
                isOutWater = true;
            }
            
            

        }
    }

    

    public override void effectBehavior()
    {
        if (!isOutWater)
        {
            if (!ph.isDead) timer += Time.deltaTime;

            if (duration > 0)
            {
                duration -= Time.deltaTime;
            }
            else
            {
                if (!ph.isDead)
                {
                    if (damageRate <= 0)
                    {
                        ph.TakeDamage(magnitude, "Effect");

                        damageRate = drMax;

                    }
                    else
                    {
                        damageRate -= Time.deltaTime;

                    }
                }
            }
        }
        
        
        


    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidDoorHandler : MonoBehaviour
{
    private keyHandler kh;

    public enum FluidType
    {
        water,
        lava
    }

    public FluidType ft;
    private void Start()
    {
        kh = FindObjectOfType<keyHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EffectBase eb = GetComponent<EffectBase>();
        if (collision.transform.CompareTag("Player"))
        {
            if (ft == FluidType.water)
            {
                if (kh.hasWaterKey)
                {
                    eb.effectStart = false;
                    eb.magnitude = 0;
                }
            }
            else if (ft == FluidType.lava)
            {
                if (kh.hasLavaKey)
                {
                    eb.effectStart = false;
                    eb.magnitude = 0;
                }

            }
        }
    }

    
}

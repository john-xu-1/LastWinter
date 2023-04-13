using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyHealthHandler : MonoBehaviour
{
    [SerializeField] Slider sd;


    public void setMaxDisplay(float maxH)
    {
        if (sd)
        {
            //Debug.Log(sd.maxValue);
            sd.maxValue = maxH;
        }
        
    }

    virtual public void Display(float playerHealth)
    {
        if (sd) sd.value = playerHealth;
    }
}

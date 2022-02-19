using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public void Damage (float damage, string type)
    {
        transform.parent.GetComponent<HealthBase>().TakeDamage(damage, type);
    }
}

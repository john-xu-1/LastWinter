using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamage : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected string type;

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<TakeDamage>().Damage(damage, type);
    }
}

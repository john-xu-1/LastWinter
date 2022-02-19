using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamage : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected string type;

    [SerializeField] protected Transform hitpoint;

    [SerializeField] protected float range;
    [SerializeField] public LayerMask enemyLayers;


    public bool isMelee = false;

    public void setDamage(float d)
    {
        damage = d;
    }



    public void DamageInflict()
    {

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitpoint.position, range, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.transform.GetComponent<TakeDamage>().Damage(damage, type);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(hitpoint.position, range);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isMelee) collision.transform.GetComponent<TakeDamage>().Damage(damage, type);
    }



}   

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
    [SerializeField] public LayerMask enemyCriticalLayer;

    [SerializeField] public bool isEnemy = false;

    

    


    public bool isMelee = false;

    public void setDamage(float d)
    {
        damage = d;
    }



    public void DamageInflict()
    {

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitpoint.position, range, enemyLayers);
        Collider2D[] hitCrits = Physics2D.OverlapCircleAll(hitpoint.position, range, enemyCriticalLayer);
        List<Collider2D> hitCritList = new List<Collider2D>();


        foreach (Collider2D crit in hitCrits)
        {
            hitCritList.Add(crit);

            Debug.Log(crit.name);
        }

        

        foreach (Collider2D enemy in hitEnemies)
        {
            //Debug.Log("aaaaaa " + enemy.name);
            if (!enemy.transform.CompareTag("bullet"))
            {
                if (enemy.gameObject.GetComponent<TakeDamage>().canCrit)
                {
                    enemy.transform.GetComponent<TakeDamage>().Damage(damage, type);
                }

            }
            
        }

        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(hitpoint.position, range);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        float damageTemp = damage;

        if (collision.transform.CompareTag("crit"))
        {
            damage = damage * 2;
        }

        if (!isMelee && !collision.transform.CompareTag("bullet") && collision.transform.GetComponent<TakeDamage>())
        {

            collision.transform.GetComponent<TakeDamage>().Damage(damage, type);


            damage = damageTemp;


            if (isEnemy) Destroy(transform.parent.gameObject);

        }
    }



}   

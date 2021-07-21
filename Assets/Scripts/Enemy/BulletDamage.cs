using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{

    public GameObject p;

    public float damage;

    public float attackRange = 1;

    public LayerMask whichcanhit;

    void Start()
    {
        p = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        inflictDamage();
    }


    public void inflictDamage()
    {
        bool isHit = Physics2D.OverlapCircle(transform.position, attackRange, whichcanhit);

        if (isHit)
        {
            toDamage(damage);

            Destroy(gameObject);
        }

    }

    public void toDamage(float damage)
    {
        if (p.GetComponent<HealthBase>())
        {
            p.GetComponent<HealthBase>().TakeDamage(damage);
        }
        else
        {
            Debug.Log("healthbase not found");
        }
    }
}

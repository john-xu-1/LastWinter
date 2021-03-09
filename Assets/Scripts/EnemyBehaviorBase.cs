using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorBase : MonoBehaviour
{
    
    void Start()
    {
        
    }

    void Update()
    {
        defaultAI();
    }

    void defaultAI()
    {
        Vector3 target = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector2 direction = (target - transform.position).normalized;

        GetComponent<Rigidbody2D>().AddForce(new Vector2(direction.x * 5, GetComponent<Rigidbody2D>().velocity.y),ForceMode2D.Force);
    }
}

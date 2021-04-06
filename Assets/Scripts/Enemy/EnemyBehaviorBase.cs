using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorBase : MonoBehaviour
{
    public GameObject p;
    public Rigidbody2D rb;
    public float angerRange = 2;
    public float speed = 5;

    void Start()
    {
        p = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        defaultAI();
    }


    public virtual void defaultAI()
    {
        Vector3 target = p.transform.position;
        Vector2 direction = (target - transform.position).normalized;

        rb.AddForce(new Vector2(direction.x * 5, rb.velocity.y),ForceMode2D.Force);
    }

    public float getDistX(Transform player, Transform self)
    {
        float dif;
        dif = Mathf.Abs(player.position.x - self.position.x);
        return dif;
    }
    public float getDistY(Transform player, Transform self)
    {
        float dif;
        dif = Mathf.Abs(player.position.y - self.position.y);
        return dif;
    }
}

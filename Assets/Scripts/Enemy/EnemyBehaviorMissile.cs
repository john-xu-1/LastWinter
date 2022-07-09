using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorMissile : EnemyBehaviorBase
{
	
	public float rotateSpeed = 200f;

	
	

    public override void defaultAI()
    {
		Vector2 direction = (Vector2)p.transform.position - rb.position;

		direction.Normalize();

		float rotateAmount = Vector3.Cross(direction, transform.up).z;

		rb.angularVelocity = -rotateAmount * rotateSpeed;

		rb.velocity = transform.up * speed;

		
	}

    void OnTriggerEnter2D(Collider2D collision)
	{
		

		if (collision.transform.CompareTag("Ground"))
        {
			Destroy(gameObject);
		}
		
		
		

	}
}

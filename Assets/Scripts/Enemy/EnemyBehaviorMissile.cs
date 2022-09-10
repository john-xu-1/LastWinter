using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorMissile : EnemyBehaviorBase
{
	
	public float rotateSpeed = 200f;
   
    private int pathIndex = 0;
    public float pathNodeBuffer = 0.1f;
    public EnemyBehaviorMissileLauncher mother;

    private Vector2 targetPos
    {
        get
        {
            return mother.path[pathIndex];
        }
    }

    protected override void ChildStart()
    {
        
    }

    

    public override void defaultAI()
    {
        if(Vector2.Distance(targetPos, transform.position) < pathNodeBuffer)
        {
            if (pathIndex < mother.path.Count - 1) pathIndex += 1;
            else
            {
                //FindPath();
            }
        }
		Vector2 direction = targetPos - rb.position;

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

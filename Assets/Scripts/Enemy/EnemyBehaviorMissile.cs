using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorMissile : EnemyBehaviorBase
{

	public float rotateSpeed = 200f;
	public int pathIndex;
	public float pathNodeBuffer = 0.1f;
	public EnemyBehaviorMissileLauncher mother;
	public bool pathChanged;
	private List<Vector2Int> path;

	private Vector2 targetPos
	{
		get
		{
			if (path == null) path = mother.path;
			return path[pathIndex];
		}

	}

	protected override void ChildStart()
	{

	}



	public override void defaultAI()
	{
		if (pathChanged && mother.path.Count > 0)
        {
			path = mother.path;
			pathIndex = mother.GetClosestIndex(transform.position);
			pathChanged = false;
        }

		Vector2 targetPos = this.targetPos + Vector2.up * 0.5f + Vector2.right * 0.5f;
		if (Vector2.Distance(targetPos, transform.position) < pathNodeBuffer)
		{
			if (pathIndex < mother.path.Count - 1) pathIndex += 1;
			else
			{
				targetPos = player.transform.position;
			}
		}
		Vector2 direction = targetPos - rb.position;


		direction.Normalize();

		float rotateAmount = Vector3.Cross(direction, transform.up).z;

		rb.angularVelocity = -rotateAmount * rotateSpeed;

		rb.velocity = transform.up * speed;



	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.transform.CompareTag("Ground"))
		{
			Destroy(gameObject);
		}

	}

    
}

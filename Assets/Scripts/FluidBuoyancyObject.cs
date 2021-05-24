using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidBuoyancyObject : MonoBehaviour
{
    public Rigidbody2D rb;

    public float depthBeforeSubmerged = 5;

    public float displacementAmount = 3f;

    

    private void FixedUpdate()
    {
        if (transform.position.y < -0.1f)
        {
            float displacementeMultiplier = Mathf.Clamp01(-transform.position.y / depthBeforeSubmerged) * displacementAmount;

            rb.AddForce(new Vector2(0f, Mathf.Abs(Physics2D.gravity.y) * displacementAmount), ForceMode2D.Force);
        }
    }

    
}

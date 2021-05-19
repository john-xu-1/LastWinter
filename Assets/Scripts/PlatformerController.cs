using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerController : PhysicsObject
{
    public float takeOffSpeed = 7;

    public float maxSpeed = 7;

    void Start()
    {
        
    }

    protected override void ComputeVelocity()
    {
        Vector2 horInput = Vector2.zero;

        horInput.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = takeOffSpeed;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y *= 0.5f;
            }
        }

        targetVelocity = horInput * maxSpeed;
    }
}

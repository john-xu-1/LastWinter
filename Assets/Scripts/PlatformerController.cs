using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerController : PhysicsObject
{

    public float takeOffSpeed
    {
        get
        {
            if (EffectorState == EffectorStates.None) return TakeOffSpeedNormal;
            else if (EffectorState == EffectorStates.LavaPartial) return TakeOffSpeedLava * LavaModifier;
            else if (EffectorState == EffectorStates.Lava) return TakeOffSpeedLava;
            else if (EffectorState == EffectorStates.WaterPartial) return TakeOffSpeedWater * WaterModifier;
            else if (EffectorState == EffectorStates.Water) return TakeOffSpeedWater;
            else return TakeOffSpeedNormal;
        }
    }

    public float maxSpeed
    {
        get
        {
            if (EffectorState == EffectorStates.None) return MaxSpeedNormal;
            else if (EffectorState == EffectorStates.LavaPartial || EffectorState == EffectorStates.Lava) return MaxSpeedLava;
            else if (EffectorState == EffectorStates.WaterPartial || EffectorState == EffectorStates.Water) return MaxSpeedWater;
            else return TakeOffSpeedNormal;
        }
    }

    public float MaxSpeedNormal = 7;
    public float TakeOffSpeedNormal = 10;

    public float WaterModifier = 2.5f;
    public float MaxSpeedWater = 3;
    public float TakeOffSpeedWater = 3;
    public float SinkWater = 2;

    public float LavaModifier = 3;
    public float MaxSpeedLava = 2;
    public float TakeOffSpeedLava = 2;
    public float SinkLava = 2;

    public bool isModifyGravity = true;

    

    private float SinkEffector
    {
        get
        {
            if (EffectorState == EffectorStates.Lava || EffectorState == EffectorStates.LavaPartial) return SinkLava;
            else return SinkWater;
        }
    }

    private float gravityModifierNormal;
    public float gravityModifierWater;
    public float gravityModifierLava;

    public float EffectorAcceleration
    {
        get
        {
            if (EffectorState == EffectorStates.Lava || EffectorState == EffectorStates.Water || velocity.y < 0) return EffectorAccelerationSubmerged;
            else return EffectorAccelerationPartial;

        }
    }
    public float EffectorAccelerationPartial = 250;
    public float EffectorAccelerationSubmerged = 5;

    

    public enum EffectorStates
    {
        None,
        Water,
        WaterPartial,
        Lava,
        LavaPartial
    }

    public EffectorStates EffectorState;

    public float specialYForce;

    public float specialXForce;

    private void Awake()
    {
        gravityModifierNormal = gravityModifer;
    }

    protected override void ComputeVelocity()
    {
        Vector2 horInput = Vector2.zero;

        horInput.x = Input.GetAxis("Horizontal");

        if (EffectorState == EffectorStates.None)
        {
            gravityModifer = gravityModifierNormal;
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
        }
        else
        {
            if (EffectorState == EffectorStates.Lava || EffectorState == EffectorStates.LavaPartial) gravityModifer = gravityModifierLava;
            else gravityModifer = gravityModifierWater;
            if (Input.GetButton("Jump"))
            {
                velocity.y = Mathf.Clamp(velocity.y + EffectorAcceleration * Time.deltaTime, -SinkEffector, takeOffSpeed);
            }
            else
            {
                velocity.y = -SinkEffector;
            }
            
        }

        velocity.y += specialYForce;
        velocity.x += specialXForce;


        targetVelocity = horInput * maxSpeed;

        

    }

    /*

    public float SpecialForce = 2;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (transform.transform.GetComponent<BulletGaeasTouch>())
        {
            
        }
    }
    */





}


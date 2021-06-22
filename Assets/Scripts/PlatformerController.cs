using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerController : PhysicsObject
{

    public float takeOffSpeed {
        get {
            if (EffectorState == EffectorStates.None) return takeoffSpeedNormal;
            else if (EffectorState == EffectorStates.LavaPartial) return takeoffSpeedLava;
            else if (EffectorState == EffectorStates.Lava) return takeoffSpeedLava * lavaModifier;
            else if (EffectorState == EffectorStates.WaterPartial) return takeoffSpeedWater;
            else if (EffectorState == EffectorStates.Water) return takeoffSpeedWater * waterModifier;
            else return takeoffSpeedNormal;
        }
    }
            

    public float maxSpeed {
        get
        {
            if (EffectorState == EffectorStates.None) return maxSpeedNormal;
            else if (EffectorState == EffectorStates.LavaPartial) return MaxSpeedLava;
            else if (EffectorState == EffectorStates.Lava) return MaxSpeedLava;
            else if (EffectorState == EffectorStates.WaterPartial) return MaxSpeedWater;
            else if (EffectorState == EffectorStates.Water) return MaxSpeedWater;
            else return maxSpeedNormal;
        }
    }



    public float SpeedTest;

    public float maxSpeedNormal = 7;

    public float takeoffSpeedNormal = 8;

    public float waterModifier = 0.8f;
    public float lavaModifier = 0.8f;

    public float MaxSpeedWater = 3;

    public float takeoffSpeedWater = 3;

    public float MaxSpeedLava = 2;

    public float takeoffSpeedLava = 2;


    public float gravityModifierNormal = 1;
    public float gravitymodifierwater;
    public float gravitymodiferlava;

    float elapsedTime;

    public float lavaPressIncrement = 0.03f;

    public float waterPressIncrement = 0.01f;


    public float pressIncrement
    {
        get
        {
            if (EffectorState == EffectorStates.None) return 0;
            else if (EffectorState == EffectorStates.LavaPartial) return lavaPressIncrement;
            else if (EffectorState == EffectorStates.Lava) return lavaPressIncrement;
            else if (EffectorState == EffectorStates.WaterPartial) return waterPressIncrement;
            else if (EffectorState == EffectorStates.Water) return waterPressIncrement;
            else return 0.01f;
        }
    }

    public float maxYSwimVel = 10;


    

    public float graphSteepness = 2;

    public enum EffectorStates
    {
        None,
        Water,
        WaterPartial,
        Lava,
        LavaPartial
    }

    public EffectorStates EffectorState;

    protected override void ComputeVelocity()
    {
        if (EffectorState == EffectorStates.None) gravityModifer = gravityModifierNormal;
        else if (EffectorState == EffectorStates.LavaPartial) gravityModifer = gravityModifierNormal;
        else if (EffectorState == EffectorStates.Lava) gravityModifer = gravitymodiferlava;
        else if (EffectorState == EffectorStates.WaterPartial) gravityModifer = gravityModifierNormal;
        else if (EffectorState == EffectorStates.Water) gravityModifer = gravitymodifierwater;
        else gravityModifer = gravityModifierNormal;

        Vector2 horInput = Vector2.zero;

        horInput.x = Input.GetAxis("Horizontal");

        

        if (Input.GetButton("Jump"))
        {
            if (EffectorState == EffectorStates.Lava || EffectorState == EffectorStates.Water)
            {
                elapsedTime += pressIncrement;
                velocity.y = Mathf.Clamp(bouyancyCurve(takeOffSpeed + elapsedTime, graphSteepness), Mathf.NegativeInfinity, maxYSwimVel);

                Debug.Log(velocity.y);
            }
            else
            {
                if (isGrounded)
                {
                    velocity.y = takeOffSpeed;
                }
                
            }
            
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y *= 0.5f;
            }
            elapsedTime = 0;
        }

        targetVelocity = horInput * maxSpeed;

        SpeedTest = rb.velocity.x;
    }


    public float bouyancyCurve (float x, float a)
    {
        float y;

        y = Mathf.Pow(a, x);

        return y;
    }
    



}

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

    private float dashSpeed;
    public float dashSpeedStart;
    private float dashTime;
    public float dashStartTime;
    int facing = 1;

    public float nextDashTime;
    public float DashRate;

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


    public int isDash = 0;

    bool isJump;


    public float jumpDashPadding = 0.1f;
    

    public GameObject dashTrail;

    public Animator anim;

    public bool isFalling = true;
    

    private void Awake()
    {
        gravityModifierNormal = gravityModifer;
        dashTime = dashStartTime;
        dashTrail.SetActive(false);
    }

    protected override void ComputeVelocity()
    {
        Vector2 horInput = Vector2.zero;

        horInput.x = Input.GetAxis("Horizontal");

        //flip
        if (horInput.x < 0)
        {
            facing = -1;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (horInput.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            facing = 1;
        }

        if (Mathf.Abs(velocity.x) > 0)
        {
            anim.SetBool("isRun", true);    

        }
        else if (Mathf.Abs(horInput.x) == 0)
        {
            Debug.Log(horInput.x);

            anim.SetBool("isRun", false);
        }

        

        if (EffectorState == EffectorStates.None)
        {
            gravityModifer = gravityModifierNormal;

            

            

            
            if (velocity.y <= 0)
            {
                anim.SetBool("isJump", false);
                
            }

            if (isDash == 0)
            {
                if (Input.GetButtonDown("Jump") && isGrounded)  
                {
                    velocity.y = takeOffSpeed;
                    isFalling = true;
                    anim.SetBool("isJump", true);
                    isJump = true;
                }
                else if (Input.GetButtonUp("Jump"))
                {
                    if (velocity.y > 0)
                    {
                        velocity.y *= 0.5f;
                    }
                    
                }

                //padding
                if (velocity.y <= jumpDashPadding && velocity.y >= 0)
                {
                    isJump = false;
                }
            }
            
            

            if (isJump == false)
            {
                

                if (isDash == 0)
                {
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        if (Time.time >= nextDashTime)
                        {
                            isDash = 1;
                            dashTrail.SetActive(true);
                            anim.SetTrigger("isDash");
                            nextDashTime = Time.time + DashRate;
                        }
                        

                    }
                    
                }
                else
                {
                    if (dashTime <= 0)
                    {
                        isDash = 0;
                        dashTrail.SetActive(false);
                        dashTime = dashStartTime;
                        dashSpeed = 0;
                    }
                    else
                    {
                        dashTime -= Time.deltaTime;
                        if (isDash == 1)
                        {
                            dashSpeed = dashSpeedStart;
                        }
                    }
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

        Vector2 tspeed = horInput * maxSpeed;

        targetVelocity = new Vector2(tspeed.x + specialXForce + (facing * dashSpeed), tspeed.y);

        

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


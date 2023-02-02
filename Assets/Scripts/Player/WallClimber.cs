using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimber : MonoBehaviour
{

    public PlatformerController pc;

    public float wallStayBuffer = 0.1f;

    void OnTriggerStay2D(Collider2D other)
    {
        if (PlayerController.canWallClimb && other.gameObject.tag == "Ground")
        {
            float inputVertical = Input.GetAxis("Vertical");
            
            pc.isClimbing = true;
            pc.velocity.y = inputVertical * pc.climbSpeed;
            pc.velocity.x = 0;

            pc.gravityModifer = 0;

            if (Mathf.Abs(inputVertical) < wallStayBuffer)
            {
                pc.velocity.y = 0;
            }


        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (PlayerController.canWallClimb && other.gameObject.tag == "Ground")
        {
            pc.gravityModifer = 2.0f;
            
        }
    }
}

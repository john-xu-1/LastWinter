using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody2D rb;
    float input;
    public float speed;

    public int extraJump;
    public int extraJumpValue;
    public bool isGrounded;
    public float jumpForce;

    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        input = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(speed * input, rb.velocity.y);

        if (isGrounded == true)
        {
            extraJump = extraJumpValue;

        }


        if (Input.GetKeyDown(KeyCode.Space) && extraJump > 0)
        {

            rb.velocity = Vector2.up * jumpForce;
            extraJump--;



        }
        else if (Input.GetKey(KeyCode.Space) && extraJump == 0 && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpForce;



            //animator.SetBool("takeOff", true);

        }
    }
}

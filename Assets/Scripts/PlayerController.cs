using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool isIdle = true;
    bool isLeft = false;
    int isIdleKey = Animator.StringToHash("isIdle");
    int isJumpingKey = Animator.StringToHash("isJumping");
    public bool canJump = true;
    bool isClimbing = false;
    public LayerMask groundMask;
    bool isLadder = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Animator a = GetComponent<Animator>();
        a.SetBool(isIdleKey, isIdle);
        a.SetBool(isJumpingKey, !canJump && !isClimbing);

        SpriteRenderer r = GetComponent<SpriteRenderer>();
        r.flipX = isLeft;
    }

    private void FixedUpdate()
    {
        isIdle = true;
        //the new velocity to apply to the character
        Vector2 physicsVelocity = Vector2.zero;
        Rigidbody2D r = GetComponent<Rigidbody2D>();
        
        //move to the left
        if (Input.GetKey(KeyCode.A)) {
            physicsVelocity.x -= 7;
            isIdle = false;
            isLeft = true;
        }

        //implement moving to the right for the D key
        if (Input.GetKey(KeyCode.D))
        {
            physicsVelocity.x += 7;
            isIdle = false;
            isLeft = false;
        }
        //this allows the player to jump, but only if canJump is true
        if (Input.GetKey(KeyCode.W)) {
            
            if (isLadder)
                isClimbing = true;
            else if(canJump)
            {
                //we're setting the absolute velocity here 
                //but we still want to carry on moving left or right.
                //so include the current horizontal velocity
                r.velocity = new Vector2(physicsVelocity.x, 10);
                canJump = false;
            }
        }
        if (isClimbing)
        {
            r.gravityScale = 0f;
            r.velocity = new Vector2(r.velocity.x, 5);
        }
        else
            r.gravityScale = 1f;
        //test the ground immediately below the Player
        //and if it tagged as a ground layer, then we allow the palyer to jump again.
        //The capsule collider is 4.8 units high, so 2.5 units "down" from its centre
        //will be just touching the floor when we are on the ground
        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), -Vector2.up, 2.5f, groundMask))
            canJump = true;
        //apply the updated velocity to the rigid body
        r.velocity = new Vector2(physicsVelocity.x, r.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
            canJump = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }
}

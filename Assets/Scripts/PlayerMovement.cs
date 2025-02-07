using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    [SerializeField] private float speed;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;

    private void Awake(){
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

    }
    private void Update() {
        //movement
        float horizontalInput = UnityEngine.Input.GetAxis("Horizontal");
        

        //wall jumping
        if(wallJumpCooldown < 0.2f)
        {
        
            if(UnityEngine.Input.GetKey(KeyCode.Space) && isGrounded())
                Jump();

            body.velocity = new Vector2(horizontalInput* speed, body.velocity.y);

            if(onWall() && !isGrounded())
            {
              body.gravityScale = 0;
              body.velocity = Vector2.zero;  
            }
            else
                body.gravityScale = 2.5f;   

        }
        else
            wallJumpCooldown += Time.deltaTime;


        //flip player when moving left-right
        if(horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        //Set animator parameters
        animator.SetBool("run", horizontalInput !=0);
        animator.SetBool("grounded", isGrounded());
        
        print(onWall());
    }

    private void Jump(){
        body.velocity = new Vector2(body.velocity.x, speed);
        animator.SetTrigger("jump");
        
    }

    private void OnCollisionEnter2D(Collision2D collision){
        
            
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float wallSlideSpeed = 0.3f;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallJumpCooldown = 0.5f;
    private float wallJumpTimer;
    private bool isWallSliding;
    private float horizontalInput;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Walking
        if (!isWallSliding)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        }

        // Flipping player horizontally when moving 
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Jumping
        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            Jump();
        }

        // Wall Slide
        if (OnWall() && !IsGrounded())
        {
            isWallSliding = true;
            body.velocity = new Vector2(body.velocity.x, -wallSlideSpeed); 
        }
        else
        {
            isWallSliding = false;
        }

        // Wall Jump
        if (wallJumpTimer > wallJumpCooldown)
        {
            if (Input.GetKey(KeyCode.Space) && isWallSliding)
            {
                WallJump();
                wallJumpTimer = 0f;
            }
        }
        else
        {
            wallJumpTimer += Time.deltaTime;
        }

        // Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", IsGrounded());
        // anim.SetBool("wallSlide", isWallSliding);
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        anim.SetTrigger("jump");
    }

    private void WallJump()
    {
        // Invert x-direction for wall jump to push player away from wall
        body.velocity = new Vector2(-transform.localScale.x * speed, jumpSpeed);
        anim.SetTrigger("jump");
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool OnWall()
    {
        RaycastHit2D raycastHit1 = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        RaycastHit2D raycastHit2 = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(-transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit1.collider != null || raycastHit2.collider != null;
    }

    public bool CanAttack() {
        return horizontalInput == 0 && IsGrounded() && !OnWall();
    }
}

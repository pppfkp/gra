using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeJumpingEnemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float colliderDistance;
    [SerializeField] private float jumpForce; // Force applied for jumping
    [SerializeField] private float jumpInterval; // Time interval between jumps
    private float cooldownTimer = Mathf.Infinity;
    private float jumpTimer = 0;

    private Health playerHealth;
    private Animator anim;
    private Rigidbody2D rb;
    private Transform player; // Reference to the player's transform
    private Vector3 initialScale; // Store the initial scale to avoid distortion

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        initialScale = transform.localScale; // Store the original scale
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;
        jumpTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            FlipTowardsPlayer(); // Ensure the enemy faces the player

            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
            }
        }

        // Make the enemy jump at regular intervals
        if (jumpTimer >= jumpInterval)
        {
            Jump();
            jumpTimer = 0;
        }
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0,
            Vector2.left,
            0,
            playerLayer
        );

        if (hit.collider != null)
        {
            player = hit.transform; // Get the player's transform
            playerHealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
        );
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            playerHealth.TakeDamage(damage);
        }
    }

    private void Jump()
    {
        if (rb != null)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Apply upward force
        }
    }

    private void FlipTowardsPlayer()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            if (direction.x > 0 && transform.localScale.x < 0 || direction.x < 0 && transform.localScale.x > 0)
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Reverse the x scale
        transform.localScale = localScale;
    }
}

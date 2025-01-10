using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWalkingEnemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range; // Patrolling range
    [SerializeField] private float attackRange; // Attack range
    [SerializeField] private int damage;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float colliderDistance;
    [SerializeField] private float walkSpeed; // Speed at which the enemy walks
    private float cooldownTimer = Mathf.Infinity;

    private Health playerHealth;
    private Animator anim;
    private Rigidbody2D rb;
    private Transform playerTransform;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight(range))
        {
            if (PlayerInSight(attackRange))
            {
                StopWalking(); // Stop the enemy when within attack range
                if (cooldownTimer >= attackCooldown)
                {
                    cooldownTimer = 0;
                    anim.SetTrigger("meleeAttack");
                }
            }
            else
            {
                WalkTowardsPlayer(); // Walk towards the player if not yet in attack range
            }
        }
        else
        {
            StopWalking(); // Stop moving when the player is out of patrolling range
        }
    }

    // Check if the player is in a specific range
    private bool PlayerInSight(float checkRange)
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * checkRange * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * checkRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
            playerTransform = hit.transform; // Save the player's transform
        }

        return hit.collider != null;
    }

    // Walk towards the player
    private void WalkTowardsPlayer()
    {
        if (playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * walkSpeed, rb.velocity.y);

            // Flip the enemy to face the player
            if (direction.x > 0 && transform.localScale.x < 0 || direction.x < 0 && transform.localScale.x > 0)
            {
                Flip();
            }

            anim.SetBool("moving", true); // Trigger walking animation
        }
    }

    // Stop walking when the player is in attack range or out of patrolling range
    private void StopWalking()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        anim.SetBool("moving", false); // Stop walking animation
    }

    // Flip the enemy's direction
    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        // Visualize patrolling range
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));

        // Visualize attack range
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * attackRange * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    // Damage the player
    private void DamagePlayer()
    {
        if (PlayerInSight(attackRange))
        {
            playerHealth.TakeDamage(damage);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedCollectible : MonoBehaviour
{
    [SerializeField] private float speedBoostValue; // How much to boost speed
    [SerializeField] private float speedBoostDuration; // How long the boost lasts

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            // Apply speed boost
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.ApplySpeedBoost(speedBoostValue, speedBoostDuration);
            }

            // Destroy the collectible
            Destroy(gameObject);
        }
    }
}

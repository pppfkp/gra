using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WelcomingDialogue : MonoBehaviour
{
    [SerializeField] private Text dialogue; // Reference to the Text UI element
    [SerializeField] private string message = "Welcome to the area!"; // The message to display

    private void Start()
    {
        if (dialogue != null)
        {
            dialogue.text = ""; // Ensure the dialogue box is initially empty
        }
    }

    // Trigger detection for entering the collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the object entering has the Player tag
        {
            if (dialogue != null)
            {
                dialogue.text = message; // Show the message
            }
        }
    }

    // Trigger detection for exiting the collider
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the object exiting has the Player tag
        {
            if (dialogue != null)
            {
                dialogue.text = ""; // Clear the message
            }
        }
    }
}

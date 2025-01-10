using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WelcomingDialogue : MonoBehaviour
{
    [SerializeField] private Text dialogue;
    [SerializeField] private string message = "Welcome to the area!"; // Przykładowa wiadomość do wyświetlenia

    private void Start()
    {
        if (dialogue != null)
        {
            dialogue.text = ""; // Czyszczenie pola dialogu
        }
    }

    // "Włączanie" wyświetlenia dialogu
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (dialogue != null)
            {
                dialogue.text = message;
            }
        }
    }

    // "Wyłączanie" wyświetlenia dialogu
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (dialogue != null)
            {
                dialogue.text = ""; // Czyszczenie wiadomości
            }
        }
    }
}

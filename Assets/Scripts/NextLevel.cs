using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NextLevel : MonoBehaviour
{
    [SerializeField] private int nextLevel;

    // Trigger detection for entering the collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the object entering has the Player tag
        {
            SceneManager.LoadScene(nextLevel);
        }
    }
}

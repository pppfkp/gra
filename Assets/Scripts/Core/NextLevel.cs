using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NextLevel : MonoBehaviour
{
    [SerializeField] private int nextLevel;

    // wykrywanie postaci w portalu i przenoszenie do następnego poziomu
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // wykrywanie czy postać weszła do portalu
        {
            SceneManager.LoadScene(nextLevel);
        }
    }
}

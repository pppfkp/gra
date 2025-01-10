using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealtCollectible : MonoBehaviour
{
    [SerializeField] private float healthValue;
    // Start is called before the first frame update

    // ustawianie wartości zdrowia na początku poziomu
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().AddHealth(healthValue);
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

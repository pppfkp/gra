using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private Text healthStatus;
    private float currentHealth;
    private Animator anim;
    private bool dead = false;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (healthStatus != null)
        {
            healthStatus.text = currentHealth.ToString();
        }


        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    TakeDamage(1);
        //}
    }

    // sterowanie otrzymywaniem obrażeń od przeciwników
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
        } else
        {
            if (!dead)
            {
                anim.SetTrigger("dead");

                //Player
                if (GetComponent<PlayerMovement>() != null)
                {
                    GetComponent<PlayerMovement>().enabled = false;

                    // Start coroutine to restart the level after the animation
                    StartCoroutine(RestartLevelAfterDeath());
                }

                //Enemy Melee
                if (GetComponent<MeleeEnemy>() != null)
                {
                    GetComponent<MeleeEnemy>().enabled = false;

                    // Start coroutine to disappear after the animation
                    StartCoroutine(DisappearAfterDeath());
                }

                //Enemy Melee
                if (GetComponent<MeleeWalkingEnemy>() != null)
                {
                    GetComponent<MeleeWalkingEnemy>().enabled = false;

                    // Start coroutine to disappear after the animation
                    StartCoroutine(DisappearAfterDeath());
                }

                //Enemy Melee
                if (GetComponent<MeleeJumpingEnemy>() != null)
                {
                    GetComponent<MeleeJumpingEnemy>().enabled = false;

                    // Start coroutine to disappear after the animation
                    StartCoroutine(DisappearAfterDeath());
                }

                dead = true;
            }
        }
    }

    // dodawanie zdrowia po zjedzeniu wiśni
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator DisappearAfterDeath()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        Destroy(gameObject); 
    }

    private IEnumerator RestartLevelAfterDeath()
    {
        // Wait for the "dead" animation to finish
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

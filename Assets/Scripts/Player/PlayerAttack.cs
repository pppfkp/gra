using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    private float attackTimer;
    private Animator anim;
    private PlayerMovement playerMovement;
    [SerializeField] private AudioClip fireballSound;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTimer > attackCooldown)
        {
            if (Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.LeftControl) && playerMovement.CanAttack())
            {
                Attack();
                attackTimer = 0f;
            }
        } else
        {
            attackTimer += Time.deltaTime;
        }

    }

    private void Attack()
    {
        anim.SetTrigger("attack");
        SoundManager.instance.PlaySound(fireballSound);
        
        //pool fireball
        var firebalIndex = GetAvailableFireball();
        if (firebalIndex is not null)
        {
            fireballs[(int)firebalIndex].transform.position = firePoint.position;
            fireballs[(int)firebalIndex].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
        } 
    }
    
    private int ?GetAvailableFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
            {
                return i;
            }
        }

        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    private float direction;
    private bool hit;
    [SerializeField] private float selfDestructionTime = 10.0f;
    private float timeElapsed;

    private BoxCollider2D boxCollider;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);
        timeElapsed += Time.deltaTime;

        if (timeElapsed > selfDestructionTime)
        {
            animator.SetTrigger("explode");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            hit = true;
            boxCollider.enabled = false;
            animator.SetTrigger("explode");

            if (collision.tag == "Enemy")
            {
                collision.GetComponent<Health>().TakeDamage(10);
            }
        }
    }

    public void SetDirection(float _direction)
    {
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX  = transform.localScale.x;

        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    public void Deactivate()
    {
        timeElapsed = 0f;
        gameObject.SetActive(false);
    }
}

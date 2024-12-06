using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementBehaviours : MonoBehaviour
{
    [SerializeField]
    private float rayRange = 0.3f;
    public bool isTouchingWall = false;
    private Rigidbody2D rb;
    private float speed = 10f;
    private float health = 100f;
    private Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        IsTouchingWall();
        Moves();  
    }

    private void IsTouchingWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, rayRange, LayerMask.GetMask("Walls"));
        isTouchingWall = (hit.collider != null);
        Debug.DrawRay(transform.position, transform.right * rayRange, isTouchingWall ? Color.red : Color.green);
    }   

    private void Moves()
    {
        
    }

    private void Hit(float dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Hit(collision.gameObject.GetComponent<ProjectileBehaviour>().GetDamage());
        }
    }
}


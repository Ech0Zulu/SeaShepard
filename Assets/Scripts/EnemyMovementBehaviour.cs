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

    private bool canShoot = true;
    private float shootCD = 1.5f;
    private float curShootCD = 0f;
    private Transform projectilSpawn;
    private float projectileSpeed = 15f;
    private float projectileDamage = 20f;
    [SerializeField]
    private GameObject projectilePrefab;

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
        GetComponent<ShootBehaviour>().UpdateCD();
        GetComponent<ShootBehaviour>().Shoot();
    }

    private void IsTouchingWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, rayRange, LayerMask.GetMask("Walls"));
        isTouchingWall = (hit.collider != null);
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


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
        UpdateCD();

        // Permet de faire bouger l'ennemi
        Moves(1, 1);

        // Permet de faire tirer l'ennemi
        Shoot();
    }

    private void IsTouchingWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, rayRange, LayerMask.GetMask("Walls"));
        isTouchingWall = (hit.collider != null);
    }   

    private void Moves(int horizontal, int vertical)
    {
        Vector2 moveInput = new Vector2(
           horizontal,
           vertical
        ).normalized;

        if (isTouchingWall && Vector2.Dot(moveInput, transform.up) > 0)
        {
            moveInput = Vector2.zero;
        }

        Vector2 move = moveInput * speed * Time.deltaTime;
        transform.position += (Vector3)move;

        if (moveInput != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
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

    private void Shoot()
    {
        if (canShoot)
        {
            canShoot = false;
            projectilSpawn = transform;

            // Définit la rotationactuelle du joueur
            Quaternion rotation = transform.rotation;

            // Calculer les positions locales des projectiles (gauche et droite)
            Vector2 offsetLeft = (Vector2)(rotation * Vector2.up) * 1f;
            Vector2 offsetRight = (Vector2)(rotation * Vector2.down) * 1f;

            // Instancier et tirer les projectiles
            SpawnProjectile((Vector2)transform.position + offsetLeft, rotation * Vector2.up);
            SpawnProjectile((Vector2)transform.position + offsetRight, rotation * Vector2.down);
        }
    }

    private void SpawnProjectile(Vector2 spawnPosition, Vector2 direction)
    {
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, transform.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        projectile.GetComponent<ProjectileBehaviour>().SetDamage(projectileDamage);

        if (rb != null)
        {
            rb.velocity = direction.normalized * projectileSpeed;
        }
    }

    private void UpdateCD()
    {
        if (!canShoot)
        {
            curShootCD += Time.deltaTime;
            if (curShootCD >= shootCD)
            {
                canShoot = true;
                curShootCD = 0;
            }
        }
    }

    public float getHealth()
    {
        return health;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementBehaviours : MonoBehaviour
{
    public enum State
    {
        Waiting,
        Idle,
        Chasing,
        Shooting
    }

    private float rayRange = 2.3f;
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

    private State currentState = State.Waiting;

    private Transform player; // Référence au joueur (sera assignée dynamiquement)
    private float detectionRange = 5f; // Distance de détection du joueur
    private float shootingRange = 2f; // Portée pour tirer
    private float changeDirectionTime = 2f; // Temps avant de changer de direction en idle

    private float stateTimer = 0f; // Timer pour gérer les transitions d'état
    private Vector2 moveDirection; // Direction actuelle du bateau
    private float directionChangeTimer = 0f; // Timer pour changer la direction en idle

    [SerializeField]
    private GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        IsTouchingWall();
        UpdateCD();

        // Gestion des états
        switch (currentState)
        {
            case State.Waiting:
                HandleWaiting();
                break;
            case State.Idle:
                HandleIdle();
                break;
            case State.Chasing:
                HandleChasing();
                break;
            case State.Shooting:
                HandleShooting();
                break;
        }
    }

    private void IsTouchingWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, rayRange, LayerMask.GetMask("Walls"));
        isTouchingWall = (hit.collider != null);
    }   

    private void Moves(float horizontal, float vertical)
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

            // D�finit la rotationactuelle du joueur
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

    private void HandleWaiting()
    {
        stateTimer += Time.deltaTime;
        if (stateTimer > 3f) // Attendre 3 secondes
        {
            TransitionToState(State.Idle); // Passe à l'état Idle
        }
    }

    private void HandleIdle()
    {
        directionChangeTimer += Time.deltaTime;

        // Change de direction après un certain temps
        if (directionChangeTimer >= changeDirectionTime)
        {
            // Change la direction aléatoirement
            float randomAngle = Random.Range(0f, 360f); // Choisir un angle aléatoire entre 0 et 360
            Moves(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            Debug.Log("Changement !");
            directionChangeTimer = 0f; // Réinitialiser le timer
        }

        // Si le joueur est détecté, on passe à l'état Chasing
        Vector2 playerPosition = player.position; // Obtient la position du joueur via le script
        if (Vector2.Distance(transform.position, playerPosition) < detectionRange)
        {
            TransitionToState(State.Chasing);
        }
    }

    private void HandleChasing()
    {
        Vector2 playerPosition = player.position;
        Vector2 direction = (playerPosition - (Vector2)transform.position).normalized;
        Moves(direction.x, direction.y);

        float distanceToPlayer = Vector2.Distance(transform.position, playerPosition);

        if (distanceToPlayer < shootingRange)
        {
            TransitionToState(State.Shooting);
        }
        else if (distanceToPlayer > detectionRange)
        {
            TransitionToState(State.Idle); // Retourne à l'état Idle si le joueur est hors de portée
        }
    }

    private void HandleShooting()
    {
        stateTimer += Time.deltaTime;

        if (stateTimer > 1f) // Tire toutes les secondes
        {
            Shoot();
            stateTimer = 0f; // Réinitialise le timer de tir
        }

        // Si le joueur s'éloigne
        Vector2 playerPosition = player.position;
        if (Vector2.Distance(transform.position, playerPosition) > shootingRange)
        {
            TransitionToState(State.Chasing);
        }
    }

    private void TransitionToState(State newState)
    {
        currentState = newState;
        stateTimer = 0f; // Réinitialise le timer à chaque transition
    }
}


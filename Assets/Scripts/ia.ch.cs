using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        Waiting,
        Idle,
        Chasing,
        Shooting
    }

    public State currentState = State.Waiting;

    public Transform player; // Référence au joueur (sera assignée dynamiquement)
    public float detectionRange = 5f; // Distance de détection du joueur
    public float shootingRange = 2f; // Portée pour tirer
    public float speed = 5f; // Vitesse de déplacement
    public float changeDirectionTime = 2f; // Temps avant de changer de direction en idle

    private float stateTimer = 0f; // Timer pour gérer les transitions d'état
    private Rigidbody2D rb; // Composant Rigidbody2D pour les déplacements
    private Vector2 moveDirection; // Direction actuelle du bateau
    private float directionChangeTimer = 0f; // Timer pour changer la direction en idle

    private PlayerMovementBehaviours playerScript; // Référence au script du joueur

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Recherche l'objet avec le tag "Player"
        playerScript = player.GetComponent<PlayerMovementBehaviours>(); // Récupère le script du joueur
        TransitionToState(State.Waiting); // Commence en mode attente
    }

    void Update()
    {
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

    void TransitionToState(State newState)
    {
        currentState = newState;
        stateTimer = 0f; // Réinitialise le timer à chaque transition
    }

    void HandleWaiting()
    {
        stateTimer += Time.deltaTime;
        if (stateTimer > 3f) // Attendre 3 secondes
        {
            TransitionToState(State.Idle); // Passe à l'état Idle
        }
    }

    void HandleIdle()
    {
        directionChangeTimer += Time.deltaTime;

        // Change de direction après un certain temps
        if (directionChangeTimer >= changeDirectionTime)
        {
            // Change la direction aléatoirement
            float randomAngle = Random.Range(0f, 360f); // Choisir un angle aléatoire entre 0 et 360
            moveDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)); // Direction dans cet angle
            Debug.Log("Changement !");
            directionChangeTimer = 0f; // Réinitialiser le timer
        }

        // Se déplacer dans la direction choisie
        rb.MovePosition((Vector2)transform.position + moveDirection * speed * Time.deltaTime);

        // Si le joueur est détecté, on passe à l'état Chasing
        Vector2 playerPosition = playerScript.GetPlayerPosition(); // Obtient la position du joueur via le script
        if (Vector2.Distance(transform.position, playerPosition) < detectionRange)
        {
            TransitionToState(State.Chasing);
        }
    }

    void HandleChasing()
    {
        Vector2 playerPosition = playerScript.GetPlayerPosition();
        Vector2 direction = (playerPosition - (Vector2)transform.position).normalized;

        rb.MovePosition((Vector2)transform.position + direction * speed * Time.deltaTime);

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

    void HandleShooting()
    {
        stateTimer += Time.deltaTime;

        if (stateTimer > 1f) // Tire toutes les secondes
        {
            Shoot();
            stateTimer = 0f; // Réinitialise le timer de tir
        }

        // Si le joueur s'éloigne
        Vector2 playerPosition = playerScript.GetPlayerPosition();
        if (Vector2.Distance(transform.position, playerPosition) > shootingRange)
        {
            TransitionToState(State.Chasing);
        }
    }

    void Shoot()
    {
        Debug.Log("Tir sur le joueur !");
        // Ajoutez ici votre logique pour tirer (par exemple, instancier un projectile).
    }
}

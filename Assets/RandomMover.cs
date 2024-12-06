using UnityEngine;

public class RandomActionMoverWithWeights : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 4f; // Vitesse de déplacement
    public float actionInterval = 0.5f; // Temps entre chaque action (en secondes)

    // Probabilités pondérées pour les actions
    [Range(0, 1)] public float moveProbability = 0.5f; // Probabilité de se déplacer
    [Range(0, 1)] public float shootProbability = 0.3f; // Probabilité de tirer
    [Range(0, 1)] public float waitProbability = 0.2f; // Probabilité d'attendre

    private Vector2 currentDirection; // Direction actuelle
    private float timeSinceLastAction; // Temps écoulé depuis la dernière action
    private string currentAction = "move"; // Action actuelle (move, shoot, wait)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        NormalizeProbabilities(); // Assurez-vous que les probabilités totalisent 1
        ChooseAction(); // Initialiser une action
    }

    void Update()
    {
        // Met à jour le temps écoulé
        timeSinceLastAction += Time.deltaTime;

        // Si l'intervalle est atteint, choisir une nouvelle action
        if (timeSinceLastAction >= actionInterval)
        {
            ChooseAction();
            timeSinceLastAction = 0f; // Réinitialiser le compteur
        }

        // Effectuer l'action actuelle
        PerformAction();
    }

    void ChooseAction()
    {
        // Générer un nombre entre 0 et 1
        float randomValue = Random.Range(0f, 1f);

        // Déterminer l'action basée sur les probabilités
        if (randomValue < moveProbability)
        {
            currentAction = "move";
            ChangeDirection(); // Si l'action est de bouger, changer de direction
        }
        else if (randomValue < moveProbability + shootProbability)
        {
            currentAction = "shoot";
        }
        else
        {
            currentAction = "wait";
        }

        Debug.Log($"Nouvelle action : {currentAction}");
    }

    void ChangeDirection()
    {
        // Choisir une direction aléatoire parmi les 4 directions principales
        int randomDirection = Random.Range(0, 4);

        switch (randomDirection)
        {
            case 0: currentDirection = Vector2.up; break;       // Haut
            case 1: currentDirection = Vector2.down; break;     // Bas
            case 2: currentDirection = Vector2.left; break;     // Gauche
            case 3: currentDirection = Vector2.right; break;    // Droite
        }

        Debug.Log($"Nouvelle direction : {currentDirection}");
    }

    void PerformAction()
    {
        if (currentAction == "move")
        {
            // Déplacer dans la direction actuelle
            transform.Translate(currentDirection * speed * Time.deltaTime);
        }
        else if (currentAction == "shoot")
        {
            // Simuler un tir
            Shoot();
        }
        else if (currentAction == "wait")
        {
            // Ne rien faire (attente)
            Debug.Log("J'attends...");
        }
    }

    void Shoot()
    {
        // Simuler une action de tir (par exemple, log ou instancier un projectile)
        Debug.Log("Tir !");
        // Ici, vous pouvez instancier un projectile si nécessaire.
    }

    void NormalizeProbabilities()
    {
        // Ajuster les probabilités pour qu'elles totalisent 1
        float total = moveProbability + shootProbability + waitProbability;
        moveProbability /= total;
        shootProbability /= total;
        waitProbability /= total;
    }
}

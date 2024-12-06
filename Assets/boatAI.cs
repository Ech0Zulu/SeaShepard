using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatAI : MonoBehaviour
{
    public BoatController boatController;
    public Transform[] waypoints; // Points de navigation
    public float detectionRange = 5f; // Pour éviter les obstacles

    private Transform targetWaypoint;
    private int currentWaypointIndex = 0;

    void Start()
    {
        targetWaypoint = waypoints[currentWaypointIndex];
    }

    void Update()
    {
        ChooseAction();
    }

    void ChooseAction()
    {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue <= 0.5f)
        {
            MoveTowardsWaypoint();
        }
        else if (randomValue <= 0.8f)
        {
            Shoot();
        }
        else
        {
            Wait();
        }
    }

    void MoveTowardsWaypoint()
    {
        if (targetWaypoint == null) return;

        Vector2 direction = (targetWaypoint.position - transform.position).normalized;

        // Éviter les obstacles
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange);
        if (hit.collider != null && hit.collider.CompareTag("Obstacle"))
        {
            AvoidObstacle(hit);
        }
        else
        {
            boatController.Move(direction);
        }

        // Vérifiez si le waypoint est atteint
        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            targetWaypoint = waypoints[currentWaypointIndex];
        }
    }

    void AvoidObstacle(RaycastHit2D hit)
    {
        Vector2 avoidanceDirection = Vector2.Perpendicular(hit.normal).normalized;
        boatController.Move(avoidanceDirection);
    }

    void Shoot()
    {
        Debug.Log("Le bateau tire !");
        // Ajoutez ici la logique pour tirer (instancier un projectile, par exemple).
    }

    void Wait()
    {
        Debug.Log("Le bateau attend.");
        boatController.Stop();
    }
}

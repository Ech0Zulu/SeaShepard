using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviours : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed = 10f;
    private float ro = 2f;
    private float health = 100f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Moves()
    {
        // Déplacement avant/arrière
        float moveInput = Input.GetAxis("Vertical"); // Z/S ou Flèche Haut/Bas
        Vector3 move = transform.forward * moveInput * speed * Time.deltaTime;

        // Rotation gauche/droite
        float turnInput = Input.GetAxis("Horizontal"); // Q/D ou Flèche Gauche/Droite
        Quaternion turn = Quaternion.Euler(0, turnInput * rotationSpeed * Time.deltaTime, 0);

        // Appliquer les mouvements
        transform.position += move;
        transform.rotation *= turn;

    }
}

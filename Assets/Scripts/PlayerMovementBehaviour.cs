using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviours : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed = 10f;
    private float moveSpeed = 10f;
    private float ro = 2f;
    private float health = 100f;
    private Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Moves();
    }

    private void Moves()
    {
        //Déplacemnts du joueur
        Vector3 move = new Vector3(
            Input.GetAxis("Horizontal") * speed * Time.deltaTime,
            Input.GetAxis("Vertical") * speed * Time.deltaTime,
            0
        );

        //Rotation du joueur
        if (move != Vector3.zero)
        {
            float angle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Appliquer les mouvements
        transform.position += move;

    }
}


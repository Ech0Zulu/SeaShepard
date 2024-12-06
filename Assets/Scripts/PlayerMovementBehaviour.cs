using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBehaviours : MonoBehaviour
{
    private float rayRange = 2.3f;
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

    // Méthode pour vérifier si le joueur touche un mur
    private void IsTouchingWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, rayRange, LayerMask.GetMask("Walls"));
        isTouchingWall = (hit.collider != null);
        Debug.DrawRay(transform.position, transform.right * rayRange, isTouchingWall ? Color.red : Color.green);
    }   

    // Méthode de déplacement du joueur
    private void Moves()
    {
        Vector2 moveInput = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
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

    // Méthode pour appliquer des dégâts au joueur
    private void Hit(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public float GetHealth()
    {
        return health;

    }

    public float GetHealth()
    {
        return health;

    }

    // Collision avec des projectiles
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Hit(collision.gameObject.GetComponent<ProjectileBehaviour>().GetDamage());
        }
    }

    // Méthode publique pour obtenir la position du joueur
    public Vector2 GetPlayerPosition()
    {
        return transform.position;
    }
}

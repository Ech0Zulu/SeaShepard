using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviours : MonoBehaviour
{
    [SerializeField]
    private float rayRange = 0.3f;
    public bool isTouchingWall = false;

    private Rigidbody2D rb;
    private float speed = 10f;
    private float health = 100f;
    private float damage = 25f;
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

    private void Hit()
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public float GetHealth()
    {
        return health;
    }
}


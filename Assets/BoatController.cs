using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 200f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 direction)
    {
        rb.velocity = direction.normalized * speed;
    }

    public void Rotate(float rotation)
    {
        rb.rotation += rotation * rotationSpeed * Time.deltaTime;
    }

    public void Stop()
    {
        rb.velocity = Vector2.zero;
    }
}

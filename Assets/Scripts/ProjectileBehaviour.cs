using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField]
    private float lifeTime = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Détruire le projectile en cas de collision
        Destroy(gameObject);
    }
}

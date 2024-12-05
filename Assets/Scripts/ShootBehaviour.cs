using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBehaviour : MonoBehaviour
{
    private bool canShoot = true;
    private float shootCD = 1.5f;
    private float curShootCD = 0f;
    private Vector2 mousePos;
    private Transform projectilSpawn;
    private float projectileSpeed = 10f;

    [SerializeField]
    private GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCD();
        Aim();
        Shoot();
    }

    private void Aim()
    {
         mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            canShoot = false;
            projectilSpawn = transform.Find("Gun");

            // Calculer la direction et l'angle du tir
            Vector2 direction = (mousePos - (Vector2)projectilSpawn.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Créer le projectile avec le bon angle de tir
            GameObject projectile = Instantiate(projectilePrefab, (Vector2)projectilSpawn.position, Quaternion.Euler(0, 0, angle));

            // Ignorer la collision avec le spawn
            Collider2D spawnCollider = projectilSpawn.GetComponent<Collider2D>();
            Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
            Collider2D playerCollider = GetComponent<Collider2D>();
            if (spawnCollider != null && projectileCollider != null)
            {
                Physics2D.IgnoreCollision(spawnCollider, projectileCollider);
            }
            if (playerCollider != null && projectileCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, projectileCollider);
            }

            // Ajouter une vitesse au projectile (si Rigidbody2D est attaché)
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * projectileSpeed;
            }
            Debug.Log("Fire !");
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

}
